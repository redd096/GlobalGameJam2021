using System.Collections;
using UnityEngine;
using redd096;

public abstract class HeadPlayer : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] int layerOnPick = 2;
    [SerializeField] int layerOnDrop = -1;

    [Header("Throw")]
    [SerializeField] LayerMask layerToBounce = default;
    [SerializeField] float decreaseSpeedEverySecond = 0.5f;
    [SerializeField] float decreaseSpeedAtBounce = 3;

    Rigidbody2D rb;

    float privateSpeed;
    float speed { get { return privateSpeed; } set { privateSpeed = Mathf.Max(0, value); } }  //can't go under 0
    Vector2 direction;
    Coroutine throwCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //bounce on hit
        Bounce(collision);
    }

    #region private API

    IEnumerator ThrowCoroutine()
    {
        while(true)
        {
            //decrease speed every fixed update
            yield return new WaitForFixedUpdate();
            speed -= decreaseSpeedEverySecond * Time.fixedDeltaTime;

            //move rigidbody
            rb.velocity = direction * speed;

            //if stopped movement, stop coroutine
            if (speed <= 0)
                yield break;
        }
    }

    void Bounce(Collider2D collision)
    {
        //if hit this layer
        if (layerToBounce.ContainsLayer(collision.gameObject.layer))
        {
            //decrease speed at bounce
            speed -= decreaseSpeedAtBounce;

            RaycastHit2D raycastHit = Physics2D.Linecast(transform.position, transform.position.SumVectors(direction), layerToBounce);

            //bounce
            direction = Vector2.Reflect(direction, raycastHit.normal);
        }
    }

    #endregion

    #region public API

    public virtual void PickHead(Transform owner)
    {
        //set layer on pick
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.sortingOrder = layerOnPick;

        //set parent
        transform.SetParent(owner);
    }

    public virtual void DropHead()
    {
        //set layer on drop
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.sortingOrder = layerOnDrop;

        //remove parent
        transform.SetParent(null);
    }

    public virtual void ThrowHead(float force, Vector2 direction)
    {
        //drop head by default
        DropHead();

        //set speed and direction
        speed = force;
        this.direction = direction;

        //start throw coroutine
        if (throwCoroutine != null)
            StopCoroutine(throwCoroutine);

        throwCoroutine = StartCoroutine(ThrowCoroutine());
    }

    #endregion
}

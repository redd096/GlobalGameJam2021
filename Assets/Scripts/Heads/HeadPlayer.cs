using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public abstract class HeadPlayer : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] int layerOnPick = 4;

    [Header("Throw")]
    [SerializeField] LayerMask layerToBounce = default;
    [SerializeField] float decreaseSpeedEverySecond = 5f;
    [SerializeField] float decreaseSpeedAtBounce = 3;

    Dictionary<SpriteRenderer, int> defaultLayers = new Dictionary<SpriteRenderer, int>();
    Rigidbody2D rb;

    float speed;
    float Speed { get { return speed; } set { speed = Mathf.Max(0, value); } }  //can't go under 0
    Vector2 direction;
    Coroutine throwCoroutine;

    //speed at 0
    public bool IsStill => Speed <= 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //set default layers
        foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            defaultLayers.Add(sprite, sprite.sortingOrder);
        }
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
            Speed -= decreaseSpeedEverySecond * Time.fixedDeltaTime;

            //move rigidbody
            rb.velocity = direction * Speed;

            //if stopped movement, stop coroutine
            if (Speed <= 0)
                yield break;
        }
    }

    void Bounce(Collider2D collision)
    {
        //if hit this layer
        if (layerToBounce.ContainsLayer(collision.gameObject.layer))
        {
            //decrease speed at bounce
            Speed -= decreaseSpeedAtBounce;

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
        foreach (SpriteRenderer sprite in defaultLayers.Keys)
            sprite.sortingOrder = layerOnPick;

        //be sure to have speed at 0
        Speed = 0;

        //set parent and position
        transform.SetParent(owner);
        transform.localPosition = Vector3.zero;
    }

    public virtual void DropHead()
    {
        //set layer on drop
        foreach (SpriteRenderer sprite in defaultLayers.Keys)
            sprite.sortingOrder = defaultLayers[sprite];

        //be sure to have speed at 0
        Speed = 0;

        //remove parent
        transform.SetParent(null);
    }

    public virtual void ThrowHead(float force, Vector2 direction)
    {
        //drop head by default
        DropHead();

        //set speed and direction
        Speed = force;
        this.direction = direction;

        //start throw coroutine
        if (throwCoroutine != null)
            StopCoroutine(throwCoroutine);

        throwCoroutine = StartCoroutine(ThrowCoroutine());
    }

    public virtual void OnPlayerCollisionEnter2D(Collision2D collision)
    {

    }

    #endregion
}

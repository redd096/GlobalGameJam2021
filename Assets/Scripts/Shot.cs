using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Shot")]
[RequireComponent(typeof(ShotGraphics))]
[SelectionBase]
public class Shot : MonoBehaviour
{
    [Header("Shot")]
    [SerializeField] LayerMask layersToDestroyShot = default;

    Rigidbody2D rb;

    Vector2 direction;
    float speed;
    float damage;
    Character owner;
    Coroutine movementCoroutine;

    public System.Action onHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float speed, float damage, Character owner)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.owner = owner;

        //start coroutine
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(MovementCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //check if hit the important head on ground
        HeadPlayer head = collision.GetComponentInParent<HeadPlayer>();
        if(head && head.HeadToEndGame && head.Owner == null)
        {
            //if is the important one and not on the owner
            onHit?.Invoke();
            head.GetDamage(damage);
            Pooling.Destroy(gameObject);
            return;
        }

        //if hit player, damage and destroy shot
        Character character = collision.GetComponentInParent<Character>();
        if(character && character != owner)
        {
            onHit?.Invoke();
            character.GetDamage(damage);
            Pooling.Destroy(gameObject);
            return;
        }

        //if hit destroy layer, destroy shot
        if (layersToDestroyShot.ContainsLayer(collision.gameObject.layer))
        {
            onHit?.Invoke();
            Pooling.Destroy(gameObject);
            return;
        }
    }

    IEnumerator MovementCoroutine()
    {
        //rotate to aim position
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //move
        while (true)
        {
            rb.velocity = direction * speed;

            yield return null;
        }
    }
}

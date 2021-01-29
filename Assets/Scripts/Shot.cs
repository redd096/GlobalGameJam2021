using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Shot")]
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
        //if hit player, damage and destroy shot
        Character character = collision.GetComponentInParent<Character>();
        if(character && character != owner)
        {
            character.GetDamage(damage);
            Pooling.Destroy(gameObject);
        }

        //if hit destroy layer, destroy shot
        if (layersToDestroyShot.ContainsLayer(collision.gameObject.layer))
        {
            Pooling.Destroy(gameObject);
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

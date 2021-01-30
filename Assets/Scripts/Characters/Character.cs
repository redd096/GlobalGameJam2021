using UnityEngine;

[RequireComponent(typeof(CharacterGraphics))]
public abstract class Character : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float health = 100;

    Vector2 directionPlayer;
    public Vector2 DirectionPlayer
    {
        get
        {
            return directionPlayer;
        }
        protected set
        {
            //set value
            if (value.magnitude > 0.1f)
            {
                directionPlayer = value;
            }
            //if no value, set right or left from previous
            else
            {
                directionPlayer = directionPlayer.x > 0 ? Vector2.right : Vector2.left;
            }
        }
    }

    public System.Action<bool> onDead;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        //get reference
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die(false);
        }
    }

    public void Die(bool falling)
    {
        AudioManager.PlaySound(gameObject);
        onDead?.Invoke(falling);
        enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public abstract void PickHead();
    public abstract void DropHead();
}

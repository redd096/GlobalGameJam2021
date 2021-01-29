using UnityEngine;

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

    public System.Action onDead;

    public void GetDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        onDead?.Invoke();
        enabled = false;
    }

    public abstract void PickHead();
    public abstract void DropHead();
}

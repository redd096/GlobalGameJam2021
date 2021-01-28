using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if(player)
        {
            Destroy(player.gameObject);
        }
    }
}

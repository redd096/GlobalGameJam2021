using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Kill Player")]
[SelectionBase]
public class KillPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if(player)
        {
            player.Die();
        }
    }
}

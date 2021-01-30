using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Kill Player")]
[SelectionBase]
public class KillPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit player, kill
        Player player = collision.GetComponentInParent<Player>();
        if(player)
        {
            player.Die(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if there is an head and is still, destroy
        HeadPlayer head = collision.GetComponentInParent<HeadPlayer>();
        if(head && head.IsStill)
        {
            head.DestroyHead(true);
        }
    }
}

using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Interactables/Lever")]
[SelectionBase]
public class Lever : Interactable
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit head
        if (collision.GetComponentInParent<HeadPlayer>())
        {
            //interact
            if (isActive)
                Deactive();
            else
                Active();
        }
    }
}

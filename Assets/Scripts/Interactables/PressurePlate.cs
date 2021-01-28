using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Interactables/Pressure Plate")]
public class PressurePlate : Interactable
{
    void OnTriggerStay2D(Collider2D collision)
    {
        //if hit head
        HeadPlayer head = collision.GetComponentInParent<HeadPlayer>();
        if (head)
        {
            //if head is still, interact
            if(head.IsStill)
            {
                Interact();
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Interactables/Pressure Plate")]
public class PressurePlate : Interactable
{
    public List<HeadPlayer> headsOnPressure = new List<HeadPlayer>();

    void OnTriggerStay2D(Collider2D collision)
    {
        //if hit head
        HeadPlayer head = collision.GetComponentInParent<HeadPlayer>();
        if (head)
        {
            //if head is still
            if(headsOnPressure.Contains(head) == false && head.IsStill)
            {
                //add to list
                headsOnPressure.Add(head);
            }
        }

        //active or deactive
        if (headsOnPressure.Count > 0)
        {
            Active();
        }
        else
        {
            Deactive();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //if hit head
        HeadPlayer head = collision.GetComponentInParent<HeadPlayer>();
        if (head)
        {
            //remove from list
            if (headsOnPressure.Contains(head))
                headsOnPressure.Remove(head);
        }

        //deactive if there aren't heads on
        if (headsOnPressure.Count <= 0)
            Deactive();
    }
}

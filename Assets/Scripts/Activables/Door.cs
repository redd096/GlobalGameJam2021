using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Door")]
public class Door : Activable
{
    public override void Active()
    {
        Destroy(gameObject);
    }
}

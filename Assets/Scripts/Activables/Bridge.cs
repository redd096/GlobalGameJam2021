﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Bridge")]
public class Bridge : Activable
{
    public override void Active()
    {
        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }
}

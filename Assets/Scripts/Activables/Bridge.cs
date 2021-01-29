﻿using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Bridge")]
[SelectionBase]
public class Bridge : Activable
{
    void Awake()
    {
        //deactive by default
        gameObject.SetActive(false);
    }

    public override void Active()
    {
        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }
}

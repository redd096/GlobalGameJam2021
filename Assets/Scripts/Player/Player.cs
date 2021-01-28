using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Player")]
public class Player : StateMachine
{
    [Header("Important")]
    public bool useAcceleration = true;

    [Header("Normal Movement")]
    public float speed = 10;

    [Header("Physics Movement")]
    public float acceleration = 10;

    void Start()
    {
        //set default state
        SetState(new MovementPlayerState());
    }
}

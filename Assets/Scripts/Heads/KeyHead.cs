﻿using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Key Head")]
[SelectionBase]
public class KeyHead : HeadPlayer
{
    [Header("Key Head")]
    [SerializeField] Activable objectToOpen = default;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //if hit object to open
        if (collision.GetComponentInParent<Activable>() == objectToOpen)
        {
            objectToOpen.Active();
        }
    }

    public override void OnPlayerCollisionEnter2D(Collision2D collision)
    {
        base.OnPlayerCollisionEnter2D(collision);

        //if hit object to open
        if(collision.gameObject.GetComponentInParent<Activable>() == objectToOpen)
        {
            objectToOpen.Active();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //draw line to object to open
        if (objectToOpen)
            Gizmos.DrawLine(transform.position, objectToOpen.transform.position);
    }
}

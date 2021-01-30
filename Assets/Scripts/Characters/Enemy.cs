﻿using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Characters/Enemy")]
[SelectionBase]
[RequireComponent(typeof(EnemyGraphics))]
public class Enemy : Character
{
    [Header("Attack Heads")]
    [SerializeField] bool attackHead = true;

    [Header("Shoot")]
    [SerializeField] float damage = 10;
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] Shot shotPrefab = default;
    [SerializeField] float speedShot = 3;
    [SerializeField] Transform shotSpawnPosition = default;

    [Header("Idle")]
    [SerializeField] float timeIdle = 1;

    float timerShot;
    Pooling<Shot> shots = new Pooling<Shot>();

    FieldOfView2D fov;

    Coroutine idleLookAroundCoroutine;
    Transform idlePoint;

    protected override void Awake()
    {
        base.Awake();

        //get fov
        fov = GetComponent<FieldOfView2D>();

        //create idle point
        idlePoint = new GameObject("Idle Point").transform;
        idlePoint.SetParent(transform);
    }

    void Update()
    {
        Transform target;
        //if player in vision
        if (CheckVision(out target))
        {
            //be sure to stop idle coroutine
            if (idleLookAroundCoroutine != null)
            {
                StopCoroutine(idleLookAroundCoroutine);
                idleLookAroundCoroutine = null;
            }

            //calculate direction
            DirectionPlayer = (target.position - shotSpawnPosition.position).normalized;
        }
        //else look around
        else
        {
            if (idleLookAroundCoroutine == null)
                idleLookAroundCoroutine = StartCoroutine(IdleLookAroundCoroutine());
        }

        //check rate of fire
        if (Time.time > timerShot)
        {
            timerShot = Time.time + rateOfFire;

            //if player in vision, shoot him
            if (target != null)
            {
                Shoot();
            }
        }
    }

    #region shoot

    bool CheckVision(out Transform target)
    {
        target = null;

        //foreach visible target
        foreach(Transform t in fov.VisibleTargets)
        {
            //if can attack head too
            if (attackHead)
            {
                //check if there is the important head on ground
                HeadPlayer head = t.GetComponentInParent<HeadPlayer>();
                if (head != null)
                {
                    //if is the important one and not on the owner
                    if (head.HeadToEndGame && head.Owner == null)
                    {
                        target = head.transform;
                        return true;
                    }
                }
            }

            //check if found player
            Player player = t.GetComponentInParent<Player>();
            if(player != null)
            {
                target = player.transform;
                return true;
            }
        }

        return false;
    }

    void Shoot()
    {
        //instantiate shot
        Shot shot = shots.Instantiate(shotPrefab, shotSpawnPosition.position, Quaternion.identity);
        shot.Init(DirectionPlayer, speedShot, damage, this);
    }

    #endregion

    #region idle

    IEnumerator IdleLookAroundCoroutine()
    {
        while (true)
        {
            //calculate points field of view
            Vector3 ViewAngleA = transform.position + fov.DirFromAngle(-fov.viewAngle / 2, false);
            Vector3 ViewAngleB = transform.position + fov.DirFromAngle(fov.viewAngle / 2, false);

            //animation to one point
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeIdle;
                idlePoint.position = Vector2.Lerp(ViewAngleA, ViewAngleB, delta);

                //look at point
                DirectionPlayer = (idlePoint.position - shotSpawnPosition.position).normalized;

                yield return null;
            }
            //back to other
            while(delta > 0)
            {
                delta -= Time.deltaTime / timeIdle;
                idlePoint.position = Vector2.Lerp(ViewAngleA, ViewAngleB, delta);

                //look at point
                DirectionPlayer = (idlePoint.position - shotSpawnPosition.position).normalized;

                yield return null;
            }
        }
    }

    #endregion

    public override void PickHead()
    {
        //
    }

    public override void DropHead()
    {
        //
    }
}

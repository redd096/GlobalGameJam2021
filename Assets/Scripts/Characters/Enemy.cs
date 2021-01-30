using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Characters/Enemy")]
[SelectionBase]
[RequireComponent(typeof(EnemyGraphics))]
public class Enemy : Character
{
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
        //if player in vision
        Player player;
        if (CheckVision(out player))
        {
            //be sure to stop idle coroutine
            if (idleLookAroundCoroutine != null)
            {
                StopCoroutine(idleLookAroundCoroutine);
                idleLookAroundCoroutine = null;
            }

            //calculate direction
            DirectionPlayer = (player.transform.position - shotSpawnPosition.position).normalized;
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
            if (CheckVision(out player))
            {
                Shoot(player);
            }
        }
    }

    #region shoot

    bool CheckVision(out Player player)
    {
        player = null;

        //foreach visible target
        foreach(Transform target in fov.VisibleTargets)
        {
            //check if found player
            player = target.GetComponentInParent<Player>();
            if(player != null)
            {
                return true;
            }
        }

        return false;
    }

    void Shoot(Player player)
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

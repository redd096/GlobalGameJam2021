using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class Enemy : Character
{
    [Header("Vision")]
    [SerializeField] float distance = 5;
    [SerializeField] Vector2 direction = Vector2.right;
    [SerializeField] float angle = 3;

    [Header("Shoot")]
    [SerializeField] float damage = 10;
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] Shot shotPrefab = default;
    [SerializeField] float speedShot = 3;
    [SerializeField] Transform shotSpawnPosition = default;

    float timerShot;
    Pooling<Shot> shots = new Pooling<Shot>();

    void Update()
    {
        //check rate of fire
        if (Time.time > timerShot)
        {
            timerShot = Time.time + rateOfFire;

            //if player in vision, shoot him
            Player player;
            if (CheckVision(out player))
            {
                Shoot(player);
            }
        }
    }

    bool CheckVision(out Player player)
    {
        player = null;



        return false;
    }

    void Shoot(Player player)
    {
        //calculate direction
        Vector2 direction = shotSpawnPosition.position - player.transform.position;
        direction.Normalize();

        //instantiate shot
        Shot shot = shots.Instantiate(shotPrefab, shotSpawnPosition.position, Quaternion.identity);
        shot.Init(direction, speedShot, damage, this);
    }

    public override void PickHead()
    {
        //
    }

    public override void DropHead()
    {
        //
    }
}

using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Characters/Enemy")]
[SelectionBase]
[RequireComponent(typeof(EnemyGraphics))]
public class Enemy : Character
{
    //[Header("Vision")]
    //[SerializeField] float distance = 5;
    //[SerializeField] Vector2 direction = Vector2.right;
    //[SerializeField] float angle = 3;

    [Header("Shoot")]
    [SerializeField] float damage = 10;
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] Shot shotPrefab = default;
    [SerializeField] float speedShot = 3;
    [SerializeField] Transform shotSpawnPosition = default;

    float timerShot;
    Pooling<Shot> shots = new Pooling<Shot>();

    FieldOfView2D fov;

    protected override void Awake()
    {
        base.Awake();

        //get fov
        fov = GetComponent<FieldOfView2D>();
    }

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

        //foreach visible target
        foreach(Transform target in fov.VisibleTargets)
        {
            //check if found player
            player = target.GetComponentInParent<Player>();
            if(player != null)
            {
                Debug.Log("ok");
                return true;
            }
        }

        return false;
    }

    void Shoot(Player player)
    {
        //calculate direction
        DirectionPlayer =  player.transform.position - shotSpawnPosition.position;
        DirectionPlayer.Normalize();

        //instantiate shot
        Shot shot = shots.Instantiate(shotPrefab, shotSpawnPosition.position, Quaternion.identity);
        shot.Init(DirectionPlayer, speedShot, damage, this);
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

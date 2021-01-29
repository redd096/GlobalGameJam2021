using UnityEngine;
using redd096;
using UnityEngine.InputSystem;

[AddComponentMenu("Global Game Jam 2021/Heads/Gun Head")]
[SelectionBase]
[RequireComponent(typeof(GunHeadGraphics))]
public class GunHead : HeadPlayer
{
    [Header("Gun Head")]
    [SerializeField] float damage = 10;
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] Shot shotPrefab = default;
    [SerializeField] float speedShot = 3;
    [SerializeField] Transform shotSpawnPosition = default;

    float timerShot;
    Pooling<Shot> shots = new Pooling<Shot>();

    NewControls inputActions;

    protected override void Awake()
    {
        base.Awake();

        inputActions = new NewControls();
        inputActions.Enable();
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }

    void Update()
    {
        //if picked by player and press input, fire
        if(Owner != null && inputActions.Gameplay.Shoot.phase == InputActionPhase.Started)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //check rate of fire
        if(Time.time > timerShot)
        {
            timerShot = Time.time + rateOfFire;

            //instantiate shot
            Shot shot = shots.Instantiate(shotPrefab, shotSpawnPosition.position, Quaternion.identity);
            shot.Init(Owner.DirectionPlayer, speedShot, damage, Owner);
        }
    }
}

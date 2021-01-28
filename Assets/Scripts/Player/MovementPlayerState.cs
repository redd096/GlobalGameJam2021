using UnityEngine;
using redd096;

public class MovementPlayerState : State
{
    Player player;
    Transform transform;
    Rigidbody2D rb;

    public override void AwakeState(StateMachine stateMachine)
    {
        base.AwakeState(stateMachine);

        //get references
        player = stateMachine as Player;
        transform = stateMachine.transform;
        rb = stateMachine.GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //move with acceleration or normal speed
        if(player.useAcceleration)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(horizontal, vertical).normalized;

            rb.AddForce(direction * player.acceleration);
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 direction = new Vector2(horizontal, vertical).normalized;

            rb.velocity = direction * player.speed;
        }
    }
}

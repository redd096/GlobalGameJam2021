using UnityEngine;
using redd096;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

[AddComponentMenu("Global Game Jam 2021/Characters/Player")]
[SelectionBase]
public class Player : Character
{
    [Header("Movement")]
    [SerializeField] bool useAcceleration = true;
    [SerializeField] float speed = 10;

    [Header("Head")]
    [SerializeField] Transform headAttach = default;
    [SerializeField] float areaToPick = 1;

    [Header("Throw")]
    public bool useAim = false;
    [SerializeField] float forceThrow = 10;

    HeadPlayer headToGrab;
    HeadPlayer currentHead;
    Animator anim;

    NewControls inputActions;
    PlayerInput playerInput;

    public HeadPlayer CurrentHead => currentHead;

    void Start()
    {
        inputActions = new NewControls();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponentInChildren<Animator>();
        AddCommands();
    }

    void OnDestroy()
    {
        RemoveCommands();
    }

    private void Update()
    {
        Aim();

        if (rb.velocity.magnitude > 0.5f)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        //check can pick heads
        CheckCanPick();
    }

    void FixedUpdate()
    {        
        Movement(inputActions.Gameplay.Movement.ReadValue<Vector2>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //call on trigger enter on head
        if (currentHead)
        {
            currentHead.OnPlayerCollisionEnter2D(collision);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //draw area to pick (cube)
        Gizmos.DrawWireCube(transform.position, new Vector3(areaToPick, areaToPick, areaToPick));
    }

    #region commands

    void AddCommands()
    {
        inputActions.Enable();
        inputActions.Gameplay.PickAndDrop.performed += PickAndDrop;
        inputActions.Gameplay.Throw.performed += Throw;
        inputActions.Gameplay.Restart.performed += Restart;
    }

    void RemoveCommands()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
            inputActions.Gameplay.PickAndDrop.performed -= PickAndDrop;
            inputActions.Gameplay.Throw.performed -= Throw;
            inputActions.Gameplay.Restart.performed -= Restart;
        }
    }

    void Movement(Vector2 direction)
    {
        direction.Normalize();

        //move with acceleration or normal speed
        if (useAcceleration)
        {
            rb.AddForce(direction * speed);
        }
        else
        {
            rb.velocity = direction * speed;
        }
        //set direction player if not aiming
        if(useAim == false)
        {
            DirectionPlayer = direction;
        }
    }

    void PickAndDrop(InputAction.CallbackContext callbackContext)
    {
        //drop or pick head
        if (currentHead)
        {
            DropHead();
        }
        else
        {
            PickHead();
        }
    }

    void Throw(InputAction.CallbackContext callbackContext)
    {
        if (currentHead)
        {
            //throw head
            currentHead.ThrowHead(forceThrow, DirectionPlayer);

            //remove head
            currentHead = null;
        }
    }

    void Aim()
    {
        //if aiming
        if (useAim)
        {
            //set direction player using mouse position
            if (playerInput.currentControlScheme == inputActions.KeyboardMouseScheme.name)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputActions.Gameplay.AimMouse.ReadValue<Vector2>());
                DirectionPlayer = mousePosition.SubtractVectors(transform.position).normalized;
            }
            //or using analog
            else
            {
                DirectionPlayer = inputActions.Gameplay.AimGamepad.ReadValue<Vector2>().normalized;
            }
        }
    }

    void Restart(InputAction.CallbackContext callbackContext)
    {
        //end level passing position
        GameManager.instance.EndLevel(transform.position);

        //restart game
        SceneLoader.instance.RestartGame();
    }

    #endregion

    #region pick and drop

    void CheckCanPick()
    {
        //check only if haven't head on
        if (currentHead == null)
        {
            //check distance
            HeadPlayer head = FindObjectsOfType<HeadPlayer>().FindNearest(transform.position);
            if (Vector2.Distance(transform.position, head.transform.position) <= areaToPick)
            {
                //check is different from current
                if (headToGrab != head)
                {
                    //remove old one
                    headToGrab?.CanPick(false);

                    //set new one
                    headToGrab = head;
                    headToGrab.CanPick(true);
                }

                return;
            }
        }

        //if there is nothing in area to pick, remove old one
        if (headToGrab != null)
        {
            headToGrab.CanPick(false);
            headToGrab = null;
        }
    }

    public override void PickHead()
    {
        //if there is an head to grab
        if (headToGrab)
        {
            //set head and position
            currentHead = headToGrab;

            //pick head
            headToGrab.PickHead(this, headAttach);
        }
    }

    public override void DropHead()
    {
        //drop head
        currentHead.DropHead(false);

        //remove head
        currentHead = null;
    }

    #endregion
}

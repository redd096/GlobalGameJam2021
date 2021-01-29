using UnityEngine;
using redd096;
using UnityEngine.InputSystem;

[AddComponentMenu("Global Game Jam 2021/Characters/Player")]
[SelectionBase]
[RequireComponent(typeof(CharacterGraphics))]
public class Player : Character
{
    [Header("Movement")]
    [SerializeField] bool useAcceleration = true;
    [SerializeField] float speed = 10;

    [Header("Head")]
    [SerializeField] Transform headAttach = default;
    [SerializeField] float areaToPick = 1;

    [Header("Throw")]
    [SerializeField] bool useAim = false;
    [SerializeField] float forceThrow = 10;

    HeadPlayer currentHead;
    Rigidbody2D rb;

    NewControls inputActions;
    PlayerInput playerInput;

    void Start()
    {
        //get reference
        rb = GetComponent<Rigidbody2D>();

        inputActions = new NewControls();
        playerInput = GetComponent<PlayerInput>();
        AddCommands();
    }

    void OnDestroy()
    {
        RemoveCommands();
    }

    private void Update()
    {
        Aim();
    }

    void FixedUpdate()
    {        
        Movement(inputActions.Gameplay.Movement.ReadValue<Vector2>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //call on trigger enter on head
        if (currentHead)
            currentHead.OnPlayerCollisionEnter2D(collision);
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
    }

    void RemoveCommands()
    {
        inputActions.Disable();
        inputActions.Gameplay.PickAndDrop.performed -= PickAndDrop;
        inputActions.Gameplay.Throw.performed -= Throw;
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

    #endregion

    #region pick and drop

    public override void PickHead()
    {
        //find nearest head, check distance
        HeadPlayer head = FindObjectsOfType<HeadPlayer>().FindNearest(transform.position);
        if (Vector2.Distance(transform.position, head.transform.position) <= areaToPick)
        {
            //set head and position
            currentHead = head;

            //pick head
            head.PickHead(this, headAttach);
        }
    }

    public override void DropHead()
    {
        //drop head
        currentHead.DropHead();

        //remove head
        currentHead = null;
    }

    #endregion
}

using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Characters/Player")]
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
    [SerializeField] bool useMouse = false;
    [SerializeField] float forceThrow = 10;

    HeadPlayer currentHead;
    Rigidbody2D rb;

    void Start()
    {
        //get reference
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DirectionPlayer = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButtonDown("Fire1"))
            Interact();
        else if (Input.GetButtonDown("Fire2"))
            ThrowHead();
    }

    void FixedUpdate()
    {
        Movement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //call on trigger enter on head
        if (currentHead)
            currentHead.OnPlayerCollisionEnter2D(collision);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        //draw area to pick (cube)
        Gizmos.DrawWireCube(transform.position, new Vector3(areaToPick, areaToPick, areaToPick));
    }

    #region private API

    void Movement(float horizontal, float vertical)
    {
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        //move with acceleration or normal speed
        if (useAcceleration)
        {
            rb.AddForce(direction * speed);
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }

    void Interact()
    {
        //drop or pick head
        if(currentHead)
        {
            DropHead();
        }
        else
        {
            PickHead();
        }
    }

    void ThrowHead()
    {
        if(currentHead)
        {
            //throw head
            if (useMouse)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentHead.ThrowHead(forceThrow, mousePosition.SubtractVectors(transform.position).normalized);
            }
            else
            {
                currentHead.ThrowHead(forceThrow, DirectionPlayer);
            }

            //remove head
            currentHead = null;
        }
    }

    #region interact

    void PickHead()
    {
        //find nearest head, check distance
        HeadPlayer head = FindObjectsOfType<HeadPlayer>().FindNearest(transform.position);
        if (Vector2.Distance(transform.position, head.transform.position) <= areaToPick)
        {
            //set head and position
            currentHead = head;

            //pick head
            head.PickHead(this, headAttach);

            //call event
            onPickHead?.Invoke();
        }
    }

    void DropHead()
    {
        //drop head
        currentHead.DropHead();

        //remove head
        currentHead = null;
    }

    #endregion

    #endregion
}

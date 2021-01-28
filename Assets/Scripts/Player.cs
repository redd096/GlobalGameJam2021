using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Player")]
public class Player : MonoBehaviour
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
        if (Input.GetButtonDown("Fire1"))
            Interact();
        else if (Input.GetButtonDown("Fire2"))
            ThrowHead(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);
    }

    void FixedUpdate()
    {
        Movement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

    void ThrowHead(Vector2 direction)
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
                currentHead.ThrowHead(forceThrow, direction.magnitude > 0.1f ? direction : Vector2.right);

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
            currentHead.transform.position = headAttach.position;

            //pick head
            head.PickHead(transform);
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

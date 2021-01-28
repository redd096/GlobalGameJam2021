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
    }

    void FixedUpdate()
    {
        Movement();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        //draw area to pick (cube)
        Gizmos.DrawWireCube(transform.position, new Vector3(areaToPick, areaToPick, areaToPick));
    }

    #region private API

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
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
        if(currentHead)
        {
            //detach head
            currentHead.DetachHead();

            //remove head and parent
            currentHead.transform.SetParent(null);
            currentHead = null;
        }
        else
        {
            //Physics2D.OverlapBox(transform.position, areaToPick, )

            //find nearest head, check distance
            HeadPlayer head = FindObjectsOfType<HeadPlayer>().FindNearest(transform.position);
            if(Vector2.Distance(transform.position, head.transform.position) <= areaToPick)
            {
                //attach head
                head.AttachHead();

                //set head and parent
                currentHead = head;
                currentHead.transform.SetParent(transform);
                currentHead.transform.position = headAttach.position;
            }
        }
    }

    #endregion
}

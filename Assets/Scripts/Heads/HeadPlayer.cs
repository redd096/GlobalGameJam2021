﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[RequireComponent(typeof(HeadGraphics))]
public abstract class HeadPlayer : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] int layerOnPick = 4;

    [Header("Throw")]
    [SerializeField] LayerMask layerToBounce = default;
    [SerializeField] LayerMask layerToDestroy = default;
    [SerializeField] float decreaseSpeedEverySecond = 5f;
    [SerializeField] float decreaseSpeedAtBounce = 3;

    Dictionary<SpriteRenderer, int> defaultLayers = new Dictionary<SpriteRenderer, int>();
    Rigidbody2D rb;

    float speed;
    float Speed { get { return speed; } set { speed = Mathf.Max(0, value); } }  //can't go under 0
    Vector2 direction;
    Coroutine throwCoroutine;

    Character owner;

    public bool IsStill => Speed <= 0;
    public Character Owner => owner;

    public System.Action onPickHead;
    public System.Action onDestroyHead;
    public System.Action<bool> onCanPick;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //set default layers
        foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            defaultLayers.Add(sprite, sprite.sortingOrder);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //bounce on hit
        CheckHit(collision);
    }

    #region private API

    IEnumerator ThrowCoroutine()
    {
        while(true)
        {
            //decrease speed every fixed update
            yield return new WaitForFixedUpdate();
            Speed -= decreaseSpeedEverySecond * Time.fixedDeltaTime;

            //move rigidbody
            rb.velocity = direction * Speed;

            //if stopped movement, stop coroutine
            if (Speed <= 0)
                yield break;
        }
    }

    void CheckHit(Collider2D collision)
    {
        //if hit lever, stop movement
        if(collision.GetComponentInParent<Lever>())
        {
            Speed = 0;
            return;
        }

        //if hit bounce layer
        if (layerToBounce.ContainsLayer(collision.gameObject.layer))
        {
            //decrease speed at bounce
            Speed -= decreaseSpeedAtBounce;

            RaycastHit2D raycastHit = Physics2D.Linecast(transform.position, transform.position.SumVectors(direction), layerToBounce);

            //bounce
            direction = Vector2.Reflect(direction, raycastHit.normal);
        }

        //if hit destroy layer
        if(layerToDestroy.ContainsLayer(collision.gameObject.layer))
        {
            DestroyHead();
        }
    }

    #endregion

    #region public API

    public void CanPick(bool canPick)
    {
        onCanPick?.Invoke(canPick);
    }

    public virtual void PickHead(Character owner, Transform headAttach)
    {
        //set layer on pick
        foreach (SpriteRenderer sprite in defaultLayers.Keys)
            sprite.sortingOrder = layerOnPick;

        //be sure to have speed at 0
        Speed = 0;
        this.owner = owner;

        //set parent and position
        transform.SetParent(headAttach);
        transform.localPosition = Vector3.zero;

        //event
        onPickHead?.Invoke();
    }

    public virtual void DropHead()
    {
        //set layer on drop
        foreach (SpriteRenderer sprite in defaultLayers.Keys)
            sprite.sortingOrder = defaultLayers[sprite];

        //be sure to have speed at 0
        Speed = 0;
        this.owner = null;

        //remove parent
        transform.SetParent(null);
    }

    public virtual void ThrowHead(float force, Vector2 direction)
    {
        //drop head by default
        DropHead();

        //set speed and direction
        Speed = force;
        this.direction = direction;

        //start throw coroutine
        if (throwCoroutine != null)
            StopCoroutine(throwCoroutine);

        throwCoroutine = StartCoroutine(ThrowCoroutine());
    }

    public virtual void OnPlayerCollisionEnter2D(Collision2D collision)
    {

    }

    public void DestroyHead()
    {
        onDestroyHead?.Invoke();
        enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    #endregion
}

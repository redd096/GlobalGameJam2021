﻿using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Graphics/Characters Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    [Header("Dead")]
    [SerializeField] float timeToDie = 2;
    [SerializeField] AnimationCurve curveRotationSpeed = default;
    [SerializeField] Tombstone tombStonePrefab = default;

    [Header("Only Player")]
    [SerializeField] Transform[] objectsToChangeAlpha = default;
    [SerializeField] float alphaOnBlackScreen = 0.3f;

    Coroutine fallingCoroutine;

    protected Character character;
    bool lookingRight;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();

        character.onDead += OnDead;

        if(character is Player)
        {
            Player player = character as Player;
            player.onInsideSpriteMask += OnInsideSpriteMask;
        }
    }

    void OnDestroy()
    {
        if (character)
        {
            character.onDead -= OnDead;

            if (character is Player)
            {
                Player player = character as Player;
                player.onInsideSpriteMask -= OnInsideSpriteMask;
            }
        }
    }

    void FixedUpdate()
    {
        //looking right and previous was left or viceversa
        if( (character.DirectionPlayer.x > 0 && lookingRight == false) || (character.DirectionPlayer.x <= 0 && lookingRight) )
        {
            RotateSprites();
        }
    }

    void OnDead(bool falling)
    {
        //death coroutine if falling
        if (falling)
        {
            if (fallingCoroutine == null)
                fallingCoroutine = StartCoroutine(FallingCoroutine());
        }
        //death by shot
        else
        {
            Destroy(gameObject);

            //instantiate tombstone
            if (tombStonePrefab)
            {
                Tombstone tombStone = Instantiate(tombStonePrefab, transform.position, Quaternion.identity);
                tombStone.Init(lookingRight);
            }
        }
    }

    void OnInsideSpriteMask(bool isInside)
    {
        //set alpha if inside or outside sprite mask
        if(isInside)
        {
            SetAlphaSprites(1);
        }
        else
        {
            SetAlphaSprites(alphaOnBlackScreen);
        }
    }

    #region private API

    void RotateSprites()
    {
        //look right
        if(character.DirectionPlayer.x > 0)
        {
            lookingRight = true;

            foreach (Transform objectToFlip in objectsToFlip)
                foreach (SpriteRenderer sprite in objectToFlip.GetComponentsInChildren<SpriteRenderer>())
                    sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
        //look left
        else
        {
            lookingRight = false;

            foreach (Transform objectToFlip in objectsToFlip)
                foreach (SpriteRenderer sprite in objectToFlip.GetComponentsInChildren<SpriteRenderer>())
                    sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
    }

    void SetAlphaSprites(float alpha)
    {
        //set alpha every object
        foreach (Transform objectToChangeAlpha in objectsToChangeAlpha)
        {
            SpriteRenderer sprite = objectToChangeAlpha.GetComponent<SpriteRenderer>();
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }
    }

    IEnumerator FallingCoroutine()
    {
        //start vars
        float delta = 0;
        Vector3 startScale = transform.localScale;

        //animation
        while (delta < 1)
        {
            delta += Time.deltaTime / timeToDie;

            //rotate and change scale
            transform.Rotate(Vector3.forward, curveRotationSpeed.Evaluate(delta));
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, delta);

            yield return null;
        }

        //destroy at the end and restart game
        Destroy(gameObject);
        SceneLoader.instance.RestartGame();
    }

    #endregion
}

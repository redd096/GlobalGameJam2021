using System.Collections;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Characters Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    [Header("Dead")]
    [SerializeField] float timeToDie = 2;
    [SerializeField] AnimationCurve curveRotationSpeed = default;

    Coroutine deadCoroutine;

    Character character;
    bool lookingRight;

    void Awake()
    {
        character = GetComponent<Character>();

        character.onDead += OnDead;
    }

    void OnDestroy()
    {
        if(character)
            character.onDead -= OnDead;
    }

    void FixedUpdate()
    {
        //looking right and previous was left or viceversa
        if( (character.DirectionPlayer.x > 0 && lookingRight == false) || (character.DirectionPlayer.x <= 0 && lookingRight) )
        {
            RotateSprites();
        }
    }

    void OnDead()
    {
        if (deadCoroutine == null)
            deadCoroutine = StartCoroutine(DeadCoroutine());
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

    IEnumerator DeadCoroutine()
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

        //destroy at the end
        Destroy(gameObject);
    }

    #endregion
}

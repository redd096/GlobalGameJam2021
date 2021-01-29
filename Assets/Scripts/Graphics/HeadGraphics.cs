using System.Collections;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Head Graphics")]
public class HeadGraphics : MonoBehaviour
{
    [Header("Head Graphics")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    [Header("Destroy")]
    [SerializeField] float timeToDestroy = 2;
    [SerializeField] AnimationCurve curveRotationSpeed = default;

    [Header("Can pick")]
    [SerializeField] GameObject hintCanPick = default;

    [Header("Throw")]
    [SerializeField] ParticleSystem trailToActive = default;

    protected HeadPlayer headPlayer;

    bool lookingRight;
    Coroutine destroyHeadCoroutine;

    void Awake()
    {
        headPlayer = GetComponent<HeadPlayer>();

        //hide hint and trail by default
        hintCanPick.SetActive(false);
        trailToActive.gameObject.SetActive(false);

        //add event
        headPlayer.onCanPick += OnCanPick;
        headPlayer.onPickHead += OnPickHead;
        headPlayer.onDestroyHead += OnDestroyHead;
        headPlayer.onThrow += OnThrow;
        headPlayer.onStop += OnStop;
    }

    void OnDestroy()
    {
        //remove event
        if (headPlayer)
        {
            headPlayer.onCanPick -= OnCanPick;
            headPlayer.onPickHead -= OnPickHead;
            headPlayer.onDestroyHead -= OnDestroyHead;
            headPlayer.onThrow -= OnThrow;
            headPlayer.onStop -= OnStop;
        }
    }

    void FixedUpdate()
    {
        if (headPlayer.Owner == null)
            return;

        //looking right and previous was left or viceversa
        if ((headPlayer.Owner.DirectionPlayer.x > 0 && lookingRight == false) || (headPlayer.Owner.DirectionPlayer.x <= 0 && lookingRight))
        {
            RotateSprites();
        }   
    }

    #region private API

    void OnCanPick(bool canPick)
    {
        //active or deactive hint
        if(hintCanPick)
        {
            hintCanPick.SetActive(canPick);
        }
    }

    protected virtual void OnPickHead()
    {
        RotateSprites();

        //reset and hide trail
        trailToActive.Clear();
        trailToActive.gameObject.SetActive(false);
    }

    void OnDestroyHead()
    {
        if (destroyHeadCoroutine == null)
            destroyHeadCoroutine = StartCoroutine(DestroyHeadCoroutine());
    }

    void OnThrow()
    {
        //active trail
        trailToActive.gameObject.SetActive(true);
        trailToActive.Play();
    }

    void OnStop()
    {
        //reset and hide trail
        trailToActive.Clear();
        trailToActive.gameObject.SetActive(false);
    }

    void RotateSprites()
    {
        //look right
        if (headPlayer.Owner.DirectionPlayer.x > 0)
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

    IEnumerator DestroyHeadCoroutine()
    {
        //start vars
        float delta = 0;
        Vector3 startScale = transform.localScale;

        //animation
        while (delta < 1)
        {
            delta += Time.deltaTime / timeToDestroy;

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

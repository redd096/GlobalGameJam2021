using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Graphics/Head Graphics")]
public class HeadGraphics : MonoBehaviour
{
    [Header("Head Graphics")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    [Header("Destroy")]
    [SerializeField] float timeToDestroy = 2;
    [SerializeField] AnimationCurve curveRotationSpeed = default;
    [SerializeField] ParticleSystem explosionParticlePrefab = default;

    [Header("Can pick")]
    [SerializeField] GameObject hintCanPick = default;

    [Header("Throw")]
    [SerializeField] ParticleSystem trailOnThrow = default;

    [Header("Drop")]
    [SerializeField] ParticleSystem prefabParticlesOnDrop = default;

    [Header("Pick")]
    [SerializeField] ParticleSystem prefabParticlesOnPick = default;

    protected HeadPlayer headPlayer;

    bool lookingRight;
    Coroutine fallingCoroutine;

    Pooling<ParticleSystem> poolParticlesOnDrop = new Pooling<ParticleSystem>();
    Pooling<ParticleSystem> poolParticlesOnPick = new Pooling<ParticleSystem>();

    void Awake()
    {
        headPlayer = GetComponent<HeadPlayer>();

        //hide hint and trail by default
        hintCanPick.SetActive(false);
        trailOnThrow.gameObject.SetActive(false);

        //add event
        headPlayer.onCanPick += OnCanPick;
        headPlayer.onPickHead += OnPickHead;
        headPlayer.onDestroyHead += OnDestroyHead;
        headPlayer.onThrow += OnThrow;
        headPlayer.onStop += OnStop;
        headPlayer.onDropHead += OnDropHead;
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
            headPlayer.onDropHead -= OnDropHead;
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
        trailOnThrow.Clear();
        trailOnThrow.gameObject.SetActive(false);

        //particles on pick
        ParticleSystem particle = poolParticlesOnPick.Instantiate(prefabParticlesOnPick, transform.position, transform.rotation);
        particle.Play();
    }

    void OnDestroyHead(bool falling)
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
            Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        }
    }

    void OnThrow()
    {
        //active trail
        trailOnThrow.gameObject.SetActive(true);
        trailOnThrow.Play();
    }

    void OnStop()
    {
        //reset and hide trail
        trailOnThrow.Clear();
        trailOnThrow.gameObject.SetActive(false);
    }

    void OnDropHead()
    {
        //particles on drop
        ParticleSystem particle = poolParticlesOnDrop.Instantiate(prefabParticlesOnDrop, transform.position, transform.rotation);
        particle.Play();
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

    IEnumerator FallingCoroutine()
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

using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Graphics/Shot Graphics")]
public class ShotGraphics : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] ParticleSystem particlesOnHit = default;
    [SerializeField] TrailRenderer trail = default;

    Pooling<ParticleSystem> poolParticlesOnHit = new Pooling<ParticleSystem>();
    Shot shot;

    void Awake()
    {
        shot = GetComponent<Shot>();

        //add events
        shot.onHit += OnHit;
    }

    private void OnDestroy()
    {
        //remove events
        if(shot)
        {
            shot.onHit -= OnHit;
        }
    }

    void OnHit()
    {
        //play particle
        ParticleSystem particle = poolParticlesOnHit.Instantiate(particlesOnHit, transform.position, transform.rotation);
        particle.Play();

        //reset trail
        trail.Clear();
    }
}

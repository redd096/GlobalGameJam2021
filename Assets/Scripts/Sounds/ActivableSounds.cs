using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Sounds/Activable Sounds")]
public class ActivableSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip[] soundOnActive = default;
    [SerializeField] AudioClip[] soundOnDeactive = default;

    protected Activable activable;

    void Awake()
    {
        activable = GetComponent<Activable>();

        //add events
        activable.onActive += OnActive;
        activable.onDeactive += OnDeactive;
    }

    void OnDestroy()
    {
        //remove events
        if(activable)
        {
            activable.onActive -= OnActive;
            activable.onDeactive -= OnDeactive;
        }
    }

    void OnActive()
    {
        //play sound on active
        AudioManager.PlaySound(gameObject, soundOnActive);
    }

    void OnDeactive()
    {
        //play sound on deactive
        AudioManager.PlaySound(gameObject, soundOnDeactive);
    }
}

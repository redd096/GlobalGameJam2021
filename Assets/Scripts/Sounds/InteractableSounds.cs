using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Sounds/Interactable Sounds")]
public class InteractableSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip[] soundOnActive = default;
    [SerializeField] AudioClip[] soundOnDeactive = default;

    protected Interactable interactable;

    void Awake()
    {
        interactable = GetComponent<Interactable>();

        //add events
        interactable.onChangeState += OnChangeState;
    }

    void OnDestroy()
    {
        //remove events
        if (interactable)
        {
            interactable.onChangeState -= OnChangeState;
        }
    }

    void OnChangeState(bool isActive)
    {
        if(isActive)
        {
            //play sound on active
            AudioManager.PlaySound(gameObject, soundOnActive);
        }
        else
        {
            //play sound on deactive
            AudioManager.PlaySound(gameObject, soundOnDeactive);
        }
    }
}

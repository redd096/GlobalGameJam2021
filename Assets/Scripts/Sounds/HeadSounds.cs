using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Sounds/Head Sounds")]
public class HeadSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip[] soundOnCanPick = default;
    [SerializeField] AudioClip[] soundOnPickHead = default;
    [SerializeField] AudioClip[] soundOnFallingHead = default;
    [SerializeField] AudioClip[] soundOnDestroyHead = default;
    [SerializeField] AudioClip[] soundOnThrow = default;
    [SerializeField] AudioClip[] soundOnStopMovement = default;
    [SerializeField] AudioClip[] soundOnDropHead = default;

    [Header("Only Gun Head")]
    [SerializeField] AudioClip[] soundOnShoot = default;

    protected HeadPlayer headPlayer;

    void Awake()
    {
        headPlayer = GetComponent<HeadPlayer>();

        //add event
        headPlayer.onCanPick += OnCanPick;
        headPlayer.onPickHead += OnPickHead;
        headPlayer.onDestroyHead += OnDestroyHead;
        headPlayer.onThrow += OnThrow;
        headPlayer.onStop += OnStop;
        headPlayer.onDropHead += OnDropHead;

        if(headPlayer is GunHead)
        {
            ((GunHead)headPlayer).onShoot += OnShoot;
        }
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

            if (headPlayer is GunHead)
            {
                ((GunHead)headPlayer).onShoot -= OnShoot;
            }
        }
    }

    void OnCanPick(bool canPick)
    {
        if (canPick)
        {
            AudioManager.PlaySound(gameObject, soundOnCanPick);
        }
    }

    void OnPickHead()
    {
        AudioManager.PlaySound(gameObject, soundOnPickHead);
    }

    void OnDestroyHead(bool falling)
    {
        if (falling)
        {
            AudioManager.PlaySound(gameObject, soundOnFallingHead);
        }
        else
        {
            AudioManager.PlaySound(gameObject, soundOnDestroyHead);
        }
    }

    void OnThrow()
    {
        AudioManager.PlaySound(gameObject, soundOnThrow);
    }

    void OnStop()
    {
        AudioManager.PlaySound(gameObject, soundOnStopMovement);
    }

    void OnDropHead()
    {
        AudioManager.PlaySound(gameObject, soundOnDropHead);
    }

    void OnShoot()
    {
        AudioManager.PlaySound(gameObject, soundOnShoot);
    }
}

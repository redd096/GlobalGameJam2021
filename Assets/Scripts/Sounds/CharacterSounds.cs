using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Sounds/Character Sounds")]
public class CharacterSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip[] soundOnFalling = default;
    [SerializeField] AudioClip[] soundOnDeadByShot = default;

    [Header("Only Enemy")]
    [SerializeField] AudioClip[] soundOnShoot = default;

    protected Character character;

    void Awake()
    {
        character = GetComponent<Character>();

        //add events
        character.onDead += OnDead;

        if (character is Enemy)
        {
            ((Enemy)character).onShoot += OnShoot;
        }
    }

    void OnDestroy()
    {
        //remove events
        if (character)
        {
            character.onDead -= OnDead;

            if (character is Enemy)
            {
                ((Enemy)character).onShoot -= OnShoot;
            }
        }
    }

    void OnDead(bool falling)
    {
        if(falling)
        {
            //play sound on falling
            AudioManager.PlaySound(gameObject, soundOnFalling);
        }
        else
        {
            //play sound on dead
            AudioManager.PlaySound(gameObject, soundOnDeadByShot);
        }
    }

    void OnShoot()
    {
        AudioManager.PlaySound(gameObject, soundOnShoot);
    }
}

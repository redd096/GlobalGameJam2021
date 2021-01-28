using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Characters Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool startRight = true;

    Character character;

    bool lookingRight;

    void Awake()
    {
        character = GetComponent<Character>();

        //add event
        character.onPickHead += OnPickHead;
    }

    void OnDestroy()
    {
        //remove event
        character.onPickHead -= OnPickHead;
    }

    void FixedUpdate()
    {
        //looking right and previous was left or viceversa
        if( (character.DirectionPlayer.x > 0 && lookingRight == false) || (character.DirectionPlayer.x <= 0 && lookingRight) )
        {
            RotateSprites();
        }
    }

    #region private API

    void OnPickHead()
    {
        RotateSprites();
    }

    void RotateSprites()
    {
        //look right
        if(character.DirectionPlayer.x > 0)
        {
            lookingRight = true;

            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
        //look left
        else
        {
            lookingRight = false;

            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
    }

    #endregion
}

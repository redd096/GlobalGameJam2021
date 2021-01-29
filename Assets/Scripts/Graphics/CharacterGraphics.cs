using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Characters Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    Character character;

    bool lookingRight;

    void Awake()
    {
        character = GetComponent<Character>();
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

    #endregion
}

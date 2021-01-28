using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Head Graphics")]
public class HeadGraphics : MonoBehaviour
{
    [Header("Head Graphics")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    protected HeadPlayer headPlayer;

    bool lookingRight;

    void Awake()
    {
        headPlayer = GetComponent<HeadPlayer>();

        //add event
        headPlayer.onPickHead += OnPickHead;
    }

    void OnDestroy()
    {
        //remove event
        headPlayer.onPickHead -= OnPickHead;
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

    protected virtual void OnPickHead()
    {
        RotateSprites();
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

    #endregion
}

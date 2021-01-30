using UnityEngine;
using redd096;
using UnityEngine.InputSystem;

[AddComponentMenu("Global Game Jam 2021/Tombstone")]
public class Tombstone : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool startRight = true;
    [SerializeField] Transform[] objectsToFlip = default;

    NewControls inputActions;

    void OnDestroy()
    {
        //remove inputs
        if (inputActions != null)
        {
            inputActions.Disable();
            inputActions.Gameplay.Restart.performed -= Restart;
        }
    }

    public void Init(bool lookingRight)
    {
        //set input for restart
        inputActions = new NewControls();
        inputActions.Enable();
        inputActions.Gameplay.Restart.performed += Restart;

        //rotate sprites
        if (lookingRight)
        {
            foreach (Transform objectToFlip in objectsToFlip)
                foreach (SpriteRenderer sprite in objectToFlip.GetComponentsInChildren<SpriteRenderer>())
                    sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
        else
        {
            foreach (Transform objectToFlip in objectsToFlip)
                foreach (SpriteRenderer sprite in objectToFlip.GetComponentsInChildren<SpriteRenderer>())
                    sprite.flipX = startRight ? !lookingRight : lookingRight;
        }
    }

    void Restart(InputAction.CallbackContext callbackContext)
    {
        //end level passing position
        GameManager.instance.EndLevel(transform.position);

        //restart game
        SceneLoader.instance.RestartGame();
    }
}

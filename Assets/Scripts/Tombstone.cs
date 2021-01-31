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

    public static Tombstone player;

    private void OnEnable()
    {
        if (inputActions != null)
            inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();
    }

    void OnDestroy()
    {
        //remove this as player
        player = null;

        //remove inputs
        if (inputActions != null)
        {
            inputActions.Disable();
            inputActions.Gameplay.Restart.performed -= Restart;
            inputActions.Gameplay.Pause.performed -= Pause;
        }
    }

    public void Init(bool lookingRight)
    {
        //set this tombstone as player
        player = this;

        //set input for restart
        inputActions = new NewControls();
        inputActions.Enable();
        inputActions.Gameplay.Restart.performed += Restart;
        inputActions.Gameplay.Pause.performed += Pause;

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
        //restart game
        SceneLoader.instance.RestartGame();
    }

    void Pause(InputAction.CallbackContext callbackContext)
    {
        //pause game
        SceneLoader.instance.PauseGame();
    }
}

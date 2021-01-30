namespace redd096
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.InputSystem;

    [AddComponentMenu("redd096/MonoBehaviours/UI Manager")]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu = default;

        NewControls inputActions;

        void Start()
        {
            PauseMenu(false);

            //set input for resume
            inputActions = new NewControls();
            inputActions.Gameplay.Resume.performed += Resume;
        }

        private void OnDestroy()
        {
            inputActions.Gameplay.Resume.performed -= Resume;
        }

        void Resume(InputAction.CallbackContext callbackContext)
        {
            //resume game
            SceneLoader.instance.ResumeGame();
        }

        public void PauseMenu(bool active)
        {
            if (pauseMenu == null)
                return;

            //active or deactive pause menu
            pauseMenu.SetActive(active);

            //active or deactive input for resume
            if (inputActions != null)
            {
                if (active)
                {
                    inputActions.Enable();
                }
                else
                {
                    inputActions.Disable();
                }
            }
        }
    }
}
namespace redd096
{
    using UnityEngine;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uiManager { get; private set; }

        protected override void SetDefaults()
        {
            //get references
            uiManager = FindObjectOfType<UIManager>();
        }

        void Update()
        {
            //if press escape or start, pause or resume game
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                if (Time.timeScale <= 0)
                    SceneLoader.instance.ResumeGame();
                else
                    SceneLoader.instance.PauseGame();
            }
        }
    }
}
namespace redd096
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    [AddComponentMenu("redd096/Singletons/Scene Loader")]
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] float timeToWaitBeforeChangeScene = 0.2f;

        Coroutine loadNewSceneCoroutine;

        /// <summary>
        /// Resume time and hide cursor
        /// </summary>
        public void ResumeGame()
        {
            //hide pause menu
            GameManager.instance.uiManager.PauseMenu(false);

            //set timeScale to 1
            //Time.timeScale = 1;

            //enable player input and hide cursor
            if (GameManager.instance.player)
                GameManager.instance.player.enabled = true;
            else if (Tombstone.player != null)
                Tombstone.player.enabled = true;
            //Utility.LockMouse(CursorLockMode.Locked);
        }

        /// <summary>
        /// Pause time and show cursor
        /// </summary>
        public void PauseGame()
        {
            //show pause menu
            GameManager.instance.uiManager.PauseMenu(true);

            //stop time
            //Time.timeScale = 0;

            //disable player input and show cursor
            if (GameManager.instance.player)
                GameManager.instance.player.enabled = false;
            else if (Tombstone.player != null)
                Tombstone.player.enabled = false;
            //Utility.LockMouse(CursorLockMode.None);
        }

        /// <summary>
        /// Exit game (works also in editor)
        /// </summary>
        public void ExitGame()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Reload this scene
        /// </summary>
        public void RestartGame()
        {
            //show cursor and set timeScale to 1
            //Utility.LockMouse(CursorLockMode.None);
            Time.timeScale = 1;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNewSceneWithoutDelay(string scene)
        {
            //show cursor and set timeScale to 1
            //Utility.LockMouse(CursorLockMode.None);
            Time.timeScale = 1;

            //load new scene
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Load new scene by name
        /// </summary>
        public void LoadNewScene(string scene)
        {
            //start coroutine
            if (instance.loadNewSceneCoroutine == null)
                instance.loadNewSceneCoroutine = instance.StartCoroutine(LoadNewSceneCoroutine(scene));
        }

        IEnumerator LoadNewSceneCoroutine(string scene)
        {
            //wait
            yield return new WaitForSeconds(instance.timeToWaitBeforeChangeScene);

            //load scene
            LoadNewSceneWithoutDelay(scene);

            instance.loadNewSceneCoroutine = null;
        }

        /// <summary>
        /// Load next scene in build settings
        /// </summary>
        public void LoadNextScene()
        {
            //show cursor and set timeScale to 1
            //Utility.LockMouse(CursorLockMode.None);
            Time.timeScale = 1;

            //load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        /// <summary>
        /// Open url
        /// </summary>
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
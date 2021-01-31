namespace redd096
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.InputSystem;

    [AddComponentMenu("redd096/MonoBehaviours/Splash Screen")]
    public class SplashScreen : MonoBehaviour
    {
        [Header("Image and Sprites")]
        [SerializeField] Image image = default;
        [SerializeField] Sprite[] spritesToUse = default;

        [Header("Fade In and Out")]
        [Min(0)]
        [SerializeField] float waitBeforeStartFadeIn = 1;
        [Min(0)]
        [SerializeField] float timeToFadeIn = 1;

        [SerializeField] bool pressToContinue = false;

        [Min(0)]
        [SerializeField] float waitBeforeStartFadeOut = 2;
        [Min(0)]
        [SerializeField] float timeToFadeOut = 1;
        [SerializeField] string nextSceneName = "Main Scene";

        [Header("Sounds")]
        [SerializeField] AudioClip[] soundPressAnyKey = default;

        NewControls inputActions;
        bool canPressButton;
        bool pressedButton;

        void Start()
        {
            //if image is null, stop here
            if (image == null)
            {
                Debug.LogError("Missing Image UI");
                return;
            }

            //bind input
            inputActions = new NewControls();
            inputActions.Enable();
            inputActions.Gameplay.AnyKeySplashScreen.performed += AnyKey;

            //start splash screen
            StartCoroutine(FadeInAndOut());
        }

        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.Gameplay.AnyKeySplashScreen.performed -= AnyKey;
            }
        }

        void AnyKey(InputAction.CallbackContext callbackContext)
        {
            //if can press button, set pressed
            if (canPressButton)
                pressedButton = true;
        }

        IEnumerator FadeInAndOut()
        {
            //foreach sprite
            foreach (Sprite sprite in spritesToUse)
            {
                //set sprite
                image.sprite = sprite;

                //start alpha to 0
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                //wait before start fade in
                yield return new WaitForSeconds(waitBeforeStartFadeIn);

                //fade in
                float delta = 0;
                while (delta < 1)
                {
                    delta += Time.deltaTime / timeToFadeIn;
                    image.Set_Fade(delta, 0, 1);

                    yield return null;
                }

                //wait until press any key down
                if (pressToContinue)
                {
                    //set can press button and wait press
                    canPressButton = true;
                    AudioManager.PlaySound(gameObject, soundPressAnyKey);

                    while (pressedButton == false)
                    {
                        yield return null;
                    }
                }

                //wait before start fade out
                yield return new WaitForSeconds(waitBeforeStartFadeOut);

                //fade out
                delta = 0;
                while (delta < 1)
                {
                    delta += Time.deltaTime / timeToFadeOut;
                    image.Set_Fade(delta, 1, 0);

                    yield return null;
                }
            }

            //load new scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
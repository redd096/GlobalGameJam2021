using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Graphics/Interactable Graphics")]
public class InteractableGraphics : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] SpriteRenderer spriteToChange = default;
    [SerializeField] Sprite spriteOn = default;
    [SerializeField] Sprite spriteOff = default;

    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();

        interactable.onChangeState += OnChangeState;
    }

    void OnDestroy()
    {
        interactable.onChangeState -= OnChangeState;
    }

    void OnChangeState(bool isActive)
    {
        //if there is a sprite to change
        if (spriteToChange)
        {
            //set on/off
            Sprite sprite = isActive ? spriteOn : spriteOff;
            spriteToChange.sprite = sprite;
        }
    }
}

using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Normal Head")]
public class NormalHead : HeadPlayer
{
    [Header("Black Sprite")]
    [SerializeField] GameObject blackSprite = default;

    public override void PickHead(Transform owner)
    {
        base.PickHead(owner);

        //remove black sprite in scene
        blackSprite.SetActive(false);
    }

    public override void DropHead()
    {
        base.DropHead();

        //active black sprite in scene
        blackSprite.SetActive(true);
    }

    public override void ThrowHead(float force, Vector2 direction)
    {
        base.ThrowHead(force, direction);
    }
}

using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Normal Head")]
[SelectionBase]
public class NormalHead : HeadPlayer
{
    [Header("Normal Head")]
    [SerializeField] GameObject blackSprite = default;

    public override void PickHead(Character owner, Transform headAttach)
    {
        base.PickHead(owner, headAttach);
        AudioManager.PlaySound(gameObject);

        //remove black sprite in scene
        if (blackSprite)
            blackSprite.SetActive(false);
    }

    public override void DropHead(bool throwed)
    {
        base.DropHead(throwed);
        AudioManager.PlaySound(gameObject);
        //active black sprite in scene
        if (blackSprite)
            blackSprite.SetActive(true);
    }
}

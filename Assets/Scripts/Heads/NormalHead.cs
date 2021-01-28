using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Normal Head")]
public class NormalHead : HeadPlayer
{
    [Header("Normal Head")]
    [SerializeField] GameObject blackSprite = default;

    public override void PickHead(Character owner, Transform headAttach)
    {
        base.PickHead(owner, headAttach);

        //remove black sprite in scene
        blackSprite.SetActive(false);
    }

    public override void DropHead()
    {
        base.DropHead();

        //active black sprite in scene
        blackSprite.SetActive(true);
    }
}

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

        //remove black sprite in scene
        if (blackSprite)
        {
            blackSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public override void DropHead(bool throwed)
    {
        base.DropHead(throwed);

        //active black sprite in scene
        if (blackSprite)
        {
            blackSprite.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}

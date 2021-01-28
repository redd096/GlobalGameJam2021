using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Normal Head")]
public class NormalHead : HeadPlayer
{
    [SerializeField] GameObject blackSprite = default;

    public override void AttachHead()
    {
        blackSprite.SetActive(false);
    }

    public override void DetachHead()
    {
        blackSprite.SetActive(true);
    }
}

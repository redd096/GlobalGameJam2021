using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Bridge")]
[SelectionBase]
public class Bridge : Activable
{
    public override void Active()
    {
        base.Active();

        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        base.Deactive();

        gameObject.SetActive(false);
    }
}

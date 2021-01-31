using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Door")]
[SelectionBase]
public class Door : Activable
{
    public override void Active()
    {
        base.Active();

        gameObject.SetActive(false);
    }

    public override void Deactive()
    {
        base.Deactive();

        gameObject.SetActive(true);
    }
}

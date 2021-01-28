using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Activables/Door")]
public class Door : Activable
{
    public override void Active()
    {
        gameObject.SetActive(false);
    }

    public override void Deactive()
    {
        gameObject.SetActive(true);
    }
}

using UnityEngine;

[RequireComponent(typeof(ActivableSounds))]
public abstract class Activable : MonoBehaviour
{
    public System.Action onActive;
    public System.Action onDeactive;

    public virtual void Active()
    {
        onActive?.Invoke();
    }

    public virtual void Deactive()
    {
        onDeactive?.Invoke();
    }
}

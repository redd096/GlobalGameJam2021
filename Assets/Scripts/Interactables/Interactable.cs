using UnityEngine;

[RequireComponent(typeof(InteractableGraphics))]
public abstract class Interactable : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] Activable[] objectsToActivate = default;

    protected bool isActive;

    public System.Action<bool> onChangeState;

    public virtual void Active()
    {
        //do only if not active
        if (isActive)
            return;

        //set active
        isActive = true;
        onChangeState?.Invoke(isActive);

        //active object
        foreach(Activable objectToActivate in objectsToActivate)
            objectToActivate.Active();
    }

    public virtual void Deactive()
    {
        //do only if active
        if (isActive == false)
            return;

        //set not active
        isActive = false;
        onChangeState?.Invoke(isActive);

        //deactive object
        foreach (Activable objectToActivate in objectsToActivate)
            objectToActivate.Deactive();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //draw line to object to activate
        if(objectsToActivate != null)
            foreach (Activable objectToActivate in objectsToActivate)
                Gizmos.DrawLine(transform.position, objectToActivate.transform.position);
    }
}

using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] Activable[] objectsToActivate = default;

    protected bool isActive;

    public virtual void Active()
    {
        //do only if not active
        if (isActive)
            return;

        isActive = true;

        //active object
        foreach(Activable objectToActivate in objectsToActivate)
            objectToActivate.Active();
    }

    public virtual void Deactive()
    {
        //do only if active
        if (isActive == false)
            return;

        isActive = false;

        //active object
        foreach (Activable objectToActivate in objectsToActivate)
            objectToActivate.Deactive();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //draw line to object to activate
        foreach (Activable objectToActivate in objectsToActivate)
            Gizmos.DrawLine(transform.position, objectToActivate.transform.position);
    }
}

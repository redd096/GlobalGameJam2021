using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] protected Activable objectToActivate = default;

    protected bool isActive;

    public virtual void Active()
    {
        //do only if not active
        if (isActive)
            return;

        isActive = true;

        //active object
        if(objectToActivate)
            objectToActivate.Active();
    }

    public virtual void Deactive()
    {
        //do only if active
        if (isActive == false)
            return;

        isActive = false;

        //active object
        if (objectToActivate)
            objectToActivate.Deactive();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        //draw line to object to activate
        if(objectToActivate)
            Gizmos.DrawLine(transform.position, objectToActivate.transform.position);
    }
}

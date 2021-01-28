using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] protected Activable ObjectToActivate = default;

    protected bool isActive;

    public virtual void Active()
    {
        //do only if not active
        if (isActive)
            return;

        isActive = true;

        //active object
        if(ObjectToActivate)
            ObjectToActivate.Active();
    }

    public virtual void Deactive()
    {
        //do only if active
        if (isActive == false)
            return;

        isActive = false;

        //active object
        if (ObjectToActivate)
            ObjectToActivate.Deactive();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        //draw line to object to activate
        if(ObjectToActivate)
            Gizmos.DrawLine(transform.position, ObjectToActivate.transform.position);
    }
}

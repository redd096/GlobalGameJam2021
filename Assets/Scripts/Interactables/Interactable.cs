using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] protected Activable ObjectToActivate = default;

    public virtual void Interact()
    {
        //active object
        if(ObjectToActivate)
            ObjectToActivate.Active();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        //draw line to object to activate
        if(ObjectToActivate)
            Gizmos.DrawLine(transform.position, ObjectToActivate.transform.position);
    }
}

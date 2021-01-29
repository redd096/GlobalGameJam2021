using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/Heads/Key Head")]
[SelectionBase]
public class KeyHead : HeadPlayer
{
    [Header("Key Head")]
    [SerializeField] bool canOpenWhenThrowed = true;
    [SerializeField] Activable objectToOpen = default;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //if hit object to open
        if (collision.GetComponentInParent<Activable>() == objectToOpen)
        {
            OpenDoor();
        }
    }

    public override void OnPlayerCollisionEnter2D(Collision2D collision)
    {
        base.OnPlayerCollisionEnter2D(collision);

        //if hit object to open
        if(collision.gameObject.GetComponentInParent<Activable>() == objectToOpen)
        {
            OpenDoor();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //draw line to object to open
        if (objectToOpen)
            Gizmos.DrawLine(transform.position, objectToOpen.transform.position);
    }

    void OpenDoor()
    {
        //if can't open when throwed and is throwed (no owner), do nothing
        if (canOpenWhenThrowed == false && Owner == null)
            return;

        //open
        objectToOpen.Active();

        //then release head and destroy
        if(Owner != null)
            Owner.DropHead();

        Destroy(gameObject);

    }
}

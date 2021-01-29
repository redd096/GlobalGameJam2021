using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Heads/Gun Head")]
[SelectionBase]
[RequireComponent(typeof(GunHeadGraphics))]
public class GunHead : HeadPlayer
{
    [Header("Gun Head")]
    [SerializeField] float damage = 10;
    [SerializeField] float rateOfFire = 0.1f;
    [SerializeField] LineRenderer linePrefab = default;
    [SerializeField] float durationLine = 0.2f;

    float timerShot;

    Pooling<LineRenderer> lines = new Pooling<LineRenderer>();

    void Update()
    {
        //if picked by player and press input, fire
        if(owner != null && Input.GetButton("Jump"))
        {
            Fire();
        }
    }

    void Fire()
    {
        //check rate of fire
        if(Time.time > timerShot)
        {
            timerShot = Time.time + rateOfFire;
            Vector2 endPoint = owner.DirectionPlayer * 100;     //default end point for line shot

            //if hit something
            Physics2D.queriesHitTriggers = false;   //ignore trigger
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, owner.DirectionPlayer);
            if(hits.Length > 0)
            {
                RaycastHit2D hit = hits[0];
                Character character = hits[0].transform.GetComponentInParent<Character>();

                for (int i = 0; i < hits.Length; i++)
                {
                    //ignore owner and childs (this head too)
                    character = hits[i].transform.GetComponentInParent<Character>();
                    if (character && character == owner)
                    {
                        //if last thing hit, then hit nothing, return
                        if(i >= hits.Length -1)
                        {
                            ShowLine(endPoint);
                            return;
                        }

                        continue;
                    }

                    //select first hit not to ignore
                    hit = hits[i];
                    break;
                }

                endPoint = hit.point;                           //update end point line shot

                //and is character
                if(character != null && character != owner)
                {
                    //do damage
                    character.GetDamage(damage);
                }
            }

            //show shot line
            ShowLine(endPoint);
        }
    }

    void ShowLine(Vector2 endPoint)
    {
        //instantiate line
        LineRenderer l = lines.Instantiate(linePrefab, transform);
        l.SetPositions(new Vector3[2] { transform.position, endPoint });

        //coroutine to deactivate line
        StartCoroutine(DeactivateLineCoroutine(l));
    }

    IEnumerator DeactivateLineCoroutine(LineRenderer l)
    {
        //wait
        yield return new WaitForSeconds(durationLine);

        //then destroy
        Pooling.Destroy(l.gameObject);
    }
}

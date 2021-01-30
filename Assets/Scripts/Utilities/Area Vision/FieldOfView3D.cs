namespace redd096
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

#if UNITY_EDITOR

    using UnityEditor;

    [CustomEditor(typeof(FieldOfView3D))]
    public class FieldOfViewEditor3D : Editor
    {
        private void OnSceneGUI()
        {
            //draw circle
            FieldOfView3D fov = (FieldOfView3D)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

            //draw 2 line to see cone vision
            Vector3 ViewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
            Vector3 ViewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
            Handles.DrawLine(fov.transform.position, fov.transform.position + ViewAngleA * fov.viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + ViewAngleB * fov.viewRadius);

            //show targets
            Handles.color = Color.red;
            foreach(Transform visibleTarget in fov.VisibleTargets)
            {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }
        }
    }

#endif

    [AddComponentMenu("redd096/Area Vision/Field Of View 3D")]
    public class FieldOfView3D : MonoBehaviour
    {
        public float viewRadius = 10;
        [Range(0, 360)] public float viewAngle = 30;

        [SerializeField] LayerMask targetMask = default;
        [SerializeField] LayerMask obstacleMask = default;

        List<Transform> visibleTargets = new List<Transform>();
        public List<Transform> VisibleTargets => visibleTargets;

        void Start()
        {
            StartCoroutine("FindTargetsWithDelay", 0.2f);
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while(true)
            {
                //wait then find targets
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        void FindVisibleTargets()
        {
            visibleTargets.Clear();

            //find targets
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            //foreach target
            for(int i = 0; i < targetsInViewRadius.Length; i++)
            {
                //check is in angle
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    //throw raycast to see is not hide by an obstacle
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if(Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask) == false)
                    {
                        //add to list
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        #region public API

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            //direction cone vision
            if (angleIsGlobal == false)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            //direction from start direction (used to create one of two line of cone vision)
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        #endregion
    }
}
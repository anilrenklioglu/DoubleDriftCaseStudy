using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Development.Scripts.Utilities
{
    public class FOV : MonoBehaviour
    {
        [Range(0,360)]
        public float viewRadius;
        [Range(0,360)]
        public float viewAngle;

        private WaitForSeconds _wfs;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
        public List<Transform> visibleTargets = new List<Transform>();
        
        private void Start()
        {
            _wfs = new WaitForSeconds(0.2f);
            StartCoroutine(FindTargetWithDelay());
        }

        private IEnumerator FindTargetWithDelay()
        {
            while (true)
            {
                yield return _wfs;
                FindVisibleTargets();
            }    
        }
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
        
        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
    
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;

                if (target == transform) 
                {
                    continue;
                }
        
                Vector3 directionToTarget = (target.position - transform.position).normalized;
        
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
            
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
    
            Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);
    
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    
            Gizmos.color = Color.red;
            
            foreach (Transform visibleTarget in visibleTargets)
            {
                Gizmos.DrawLine(transform.position, visibleTarget.position);
            }
        }

    }
}
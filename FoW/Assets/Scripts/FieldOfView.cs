using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();


    private void Start()
    {
        StartCoroutine(findTargets(.2f));
    }

    IEnumerator findTargets(float delayTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        // all of the targets colliders within viewRadius
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            // check if the target falls within our view angle
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = (transform.position - target.position).magnitude;
                if (!Physics.Raycast(transform.position, dirToTarget,distToTarget,obstacleMask))
                {
                    // that means there are no obstacle in the way so we can see target
                    visibleTargets.Add(target);
                }

            }
        }
    }

    // take an angle and return direction of that angle

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        // in Unity unity circle degrees is different. it's not 0,90,180,270 it's 90,0,270,180
        // actually it's finding (90-x) x is real degrees say x is 0 --> 90-0 = 0 so the right sight is 90 degrees not 0 in Unity.
        // sin(90-x) = cos(x) in reality.(sin is y axis, cos is x axis) but its means u can swap sin and cos like this
        if (!angleIsGlobal)
        {
            // if angle is not global we have to convert it to global angle by adding transforms own rotation
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

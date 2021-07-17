using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    public Transform target;
    public float threshold;

    private void OnDrawGizmos() {
        if (!target) return;

        float lookDot = Vector3.Dot(transform.right, (target.position - transform.position).normalized);
        Color c = lookDot >= threshold ? Color.green : Color.red;
        Debug.DrawRay(transform.position, transform.right, c);
    }
}

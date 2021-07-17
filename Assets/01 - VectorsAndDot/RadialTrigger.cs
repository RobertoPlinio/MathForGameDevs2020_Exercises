using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    public Transform target;
    public float radius;

    private void OnDrawGizmos() {
        if (!target) return;

        Vector3 distVector = target.position - transform.position;
        bool isInside = distVector.magnitude < radius;

        Handles.color = isInside ? Color.green : Color.red;

        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
}

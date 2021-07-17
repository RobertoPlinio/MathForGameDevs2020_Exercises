using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTurret : MonoBehaviour
{
    public bool showVectors;

    public bool showWirebox;
    public float wireboxSize = 1f;

    public bool showTurret;
    public float turretHeight = 1.3f;
    public float turretBarrelLength = 0.8f;
    public float turretBarrelSeparation = 0.3f;

    private void OnDrawGizmos() {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) {
            //Finding the Right, Up and Forward vectors
            Vector3 right = Vector3.Cross(hit.normal, transform.forward).normalized;
            if (right.magnitude <= 0) right = Vector3.right;
            Vector3 forward = Vector3.Cross(right, hit.normal).normalized;

            if (showVectors) {
                Debug.DrawLine(transform.position, hit.point, Color.white);
                Debug.DrawRay(hit.point, hit.normal, Color.green);
                Debug.DrawRay(hit.point, right, Color.red);
                Debug.DrawRay(hit.point, forward, Color.blue);
            }

            //Making a local wire cube using matrix
            Matrix4x4 cubeMatrix = new Matrix4x4();
            cubeMatrix.SetColumn(0, right);
            cubeMatrix.SetColumn(1, hit.normal);
            cubeMatrix.SetColumn(2, forward);
            cubeMatrix.SetColumn(3, hit.point);
            cubeMatrix[3, 3] = 1; //Guarantees a valid TRS matrix

            if (showWirebox) {
                Debug.DrawLine(MultiplyPoint(new Vector3(1, 0, 1)), MultiplyPoint(new Vector3(-1, 0, 1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 0, 1)), MultiplyPoint(new Vector3(-1, 0, -1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 0, -1)), MultiplyPoint(new Vector3(1, 0, -1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(1, 0, -1)), MultiplyPoint(new Vector3(1, 0, 1)), Color.yellow);

                Debug.DrawLine(MultiplyPoint(new Vector3(1, 2, 1)), MultiplyPoint(new Vector3(-1, 2, 1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 2, 1)), MultiplyPoint(new Vector3(-1, 2, -1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 2, -1)), MultiplyPoint(new Vector3(1, 2, -1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(1, 2, -1)), MultiplyPoint(new Vector3(1, 2, 1)), Color.yellow);

                Debug.DrawLine(MultiplyPoint(new Vector3(1, 0, 1)), MultiplyPoint(new Vector3(1, 2, 1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 0, 1)), MultiplyPoint(new Vector3(-1, 2, 1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(1, 0, -1)), MultiplyPoint(new Vector3(1, 2, -1)), Color.yellow);
                Debug.DrawLine(MultiplyPoint(new Vector3(-1, 0, -1)), MultiplyPoint(new Vector3(-1, 2, -1)), Color.yellow);

                Vector3 MultiplyPoint(Vector3 localPoint) {
                    return cubeMatrix.MultiplyPoint3x4(localPoint * wireboxSize);
                }
            }

            //Making a two barrel turret debug
            if(showTurret) {
                Vector3 turretTopPos = hit.point + hit.normal * turretHeight;
                Vector3 leftBarrelStartPos = turretTopPos - right * turretBarrelSeparation * 0.5f;
                Vector3 rightBarrelStartPos = turretTopPos + right * turretBarrelSeparation * 0.5f;

                Debug.DrawLine(hit.point, turretTopPos, Color.magenta);
                Debug.DrawLine(leftBarrelStartPos, rightBarrelStartPos, Color.magenta);
                Debug.DrawLine(leftBarrelStartPos, leftBarrelStartPos + forward * turretBarrelLength, Color.magenta);
                Debug.DrawLine(rightBarrelStartPos, rightBarrelStartPos + forward * turretBarrelLength, Color.magenta);
            }

        } else {
            if (showVectors) Debug.DrawRay(transform.position, transform.forward, Color.cyan);
        }
    }
}

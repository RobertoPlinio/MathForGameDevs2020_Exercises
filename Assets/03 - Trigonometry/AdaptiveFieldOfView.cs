using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AdaptiveFieldOfView : MonoBehaviour
{
    public Camera cam;
    public Transform point;
    public Transform[] pointArray;
    public Point[] points;

    public bool useSinglePoint;
    [Header("Multiple points section")]
    public bool useMultiplePoints;
    public bool useRadius;
    public bool use3D;

    private void OnDrawGizmos() {
        if (!cam) return;

        //The original code
        if (useSinglePoint && point) {
            Vector3 camPoiDir = (point.position - cam.transform.position).normalized;

            float fovRAD = Mathf.Clamp(Vector3.Dot(cam.transform.forward, camPoiDir), 0f, 1f); //I had to clamp 'cause I got NaN 'cause of floating point
            fovRAD = Mathf.Acos(fovRAD);

            cam.fieldOfView = fovRAD * 2 * Mathf.Rad2Deg;
        } else {

            //Assignment 8a)
            if (useMultiplePoints) {
                if (!use3D) {
                    //2D stuff
                    if (pointArray.Length > 0 && !useRadius) {
                        float maxFOV = 0f;
                        float cachedFOV = 0f;

                        foreach (var p in pointArray) {
                            cachedFOV = GetAngle(p.position);
                            if (cachedFOV > maxFOV) maxFOV = cachedFOV;
                        }

                        cam.fieldOfView = maxFOV * 2 * Mathf.Rad2Deg;
                    } else {
                        float maxFOV = 0f;
                        float cachedFOV = 0f;

                        if (points.Length > 0 && useRadius) {
                            foreach (var p in points) {
                                Gizmos.color = Color.white;
                                Gizmos.DrawWireSphere(p.pointTrans.position, p.radius);
                                float radiusOffsetAngleRad = Mathf.Asin(p.radius / (p.pointTrans.position - cam.transform.position).magnitude);
                                cachedFOV = GetAngle(p.pointTrans.position) + radiusOffsetAngleRad;
                                if (cachedFOV > maxFOV) maxFOV = cachedFOV;
                            }

                            cam.fieldOfView = maxFOV * 2 * Mathf.Rad2Deg;
                        }
                    }
                } else {
                    //3d stuff
                    //To make it work in 3d, at least in the horizontal plane, we need to localize the points position with the camera as reference
                    //Then, we`ll flatten the points position in a place space of the camera, in this case we will remove the X axis and use their Z axis
                    // in its place. So we`ll change from X,Y,Z world plane to Z,Y plane of the camera

                    float maxFOV = float.MinValue;
                    float cachedFOV;

                    Vector2 newCamForward = Vector2.right; //Considering our local 2D space we will create to flatten the points, the new LOCAL camera forward
                    // will be the same as (1,0), so, Vector Right

                    if (points.Length > 0) {
                        foreach (var p in points) {
                            Vector3 localPointPos = cam.transform.InverseTransformPoint(p.pointTrans.position); //Point in camera space
                            Vector2 pointFlat = new Vector2(localPointPos.z, localPointPos.y); // Flattening to ZY plane

                            float distToPoint = pointFlat.magnitude;
                            Vector2 dirToPoint = pointFlat / distToPoint; //Same as doing pointFlat.normalized -> norm = vector / magnitude of vector

                            float angle = Mathf.Acos(Vector2.Dot(newCamForward, dirToPoint));
                            float pRadiusAngle = useRadius ? Mathf.Asin(p.radius / distToPoint) : 0f;
                            if (useRadius) {
                                Gizmos.color = Color.white;
                                Gizmos.DrawWireSphere(p.pointTrans.position, p.radius);
                            }

                            cachedFOV = angle + pRadiusAngle;

                            Gizmos.color = p.debugLine;
                            Gizmos.DrawLine(cam.transform.position, p.pointTrans.position);

                            //print($"{p.pointTrans.name} {cachedFOV * Mathf.Rad2Deg} angle");
                            if (cachedFOV > maxFOV) {
                                maxFOV = cachedFOV;
                            }
                        }
                    }

                    cam.fieldOfView = maxFOV * 2 * Mathf.Rad2Deg;
                }
            }
        }

        float GetAngle(Vector3 pointPos) {
            return Mathf.Acos(Mathf.Clamp(Vector3.Dot(cam.transform.forward, (pointPos - cam.transform.position).normalized), 0f, 1f));
        }
    }

    [System.Serializable]
    public struct Point
    {
        public Transform pointTrans;
        public float radius;
        public Color debugLine;
    }
}

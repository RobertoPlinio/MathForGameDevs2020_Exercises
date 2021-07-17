using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLaser : MonoBehaviour
{
    public Transform moveModel;
    public float modelTimeToReach = 1f;
    float timer;
    HitPoint moveHitPoint;
    Vector3 startPos;

    public bool cleanBounces;
    public int maxBounces;
    int bounces;

    Vector3 currentDirection;
    Vector3 currentPoint;

    public List<HitPoint> hitPoints;

    private void OnDrawGizmos() {
            bounces = 0;
        if (cleanBounces) {
            cleanBounces = false;
        }

        if (bounces < 1) {
            hitPoints.Clear();
            currentDirection = transform.right;
            currentPoint = transform.position;
        }

        while (bounces < maxBounces) {
            Ray ray = new Ray(currentPoint, currentDirection);

            if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
                hitPoints.Add(new HitPoint(currentPoint, hitInfo.point, hitInfo.normal, Vector3.Reflect(currentDirection, hitInfo.normal)));
                currentPoint = hitInfo.point;

                //Couldn't figure out by myself, damn
                currentDirection = currentDirection - 2 * (Vector3.Dot(currentDirection, hitInfo.normal) * hitInfo.normal);

                bounces++;
            } else {
                bounces = maxBounces;
            }
        } 
        foreach (var h in hitPoints) {
            Debug.DrawLine(h.initialPoint, h.hitPoint, Color.red);
        }
        Debug.DrawRay(currentPoint, currentDirection, Color.cyan);
    }

    private void Start() {
        if (hitPoints.Count > 0) {
            moveHitPoint = hitPoints[0];
            startPos = moveModel.position;
        }
    }

    private void Update() {
        if (hitPoints.Count > 0) {
            if (timer < modelTimeToReach) {
                timer += Time.deltaTime;
                moveModel.position = Vector3.Lerp(moveHitPoint.initialPoint, moveHitPoint.hitPoint, timer / modelTimeToReach);
            } else {
                timer = 0;
                moveHitPoint = hitPoints[(hitPoints.IndexOf(moveHitPoint) + 1) % hitPoints.Count];
            }
        }
    }

    [System.Serializable]
    public struct HitPoint
    {
        public Vector3 initialPoint;
        public Vector3 hitPoint;
        public Vector3 normal;
        public Vector3 expected;

        public HitPoint(Vector3 initial, Vector3 hit, Vector3 n, Vector3 e) {
            initialPoint = initial;
            hitPoint = hit;
            normal = n;
            expected = e;
        }
    }
}

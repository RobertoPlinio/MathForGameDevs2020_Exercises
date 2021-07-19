using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilAlternate : MonoBehaviour
{
    public int densityPerTurn;
    public float height;
    public float radius;
    public int turns;
    public Color startColor;
    public Color endColor;

    const float TAU = Mathf.PI * 2f;
    Vector3 AngToDir(float angRad) => new Vector3(Mathf.Cos(angRad), Mathf.Sin(angRad));
    float DirToAng(Vector2 v) => Mathf.Atan2(v.y, v.x);

    private void OnDrawGizmos() {
        float fullDensity = densityPerTurn * turns;

        for (int i = 0; i <= fullDensity; i++) {
            float angRad = i * TAU / fullDensity * turns;
            
            //Vector3 pos = AngToDir(angRad);
            //pos.z = i * height / fullDensity;
            //Gizmos.DrawSphere(transform.InverseTransformPoint(pos), 0.1f);
        }
    }

    Vector3 GetCoilLocalPosition() {
        return default;
    }
}

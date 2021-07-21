using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilAlternate : MonoBehaviour
{
    public bool useTorus = false;

    public int densityPerTurn;
    public float height;
    public float radius;
    public int turns;
    public Color startColor;
    public Color endColor;

    public float torusCircumference;

    const float TAU = Mathf.PI * 2f;
    Vector3 AngToDir(float angRad) => new Vector3(Mathf.Cos(angRad), Mathf.Sin(angRad));
    float DirToAng(Vector2 v) => Mathf.Atan2(v.y, v.x);
    
    private void OnDrawGizmos() {
        int fullDensity = densityPerTurn * turns;

        Vector3[] coil = new Vector3[fullDensity];

        for (int i = 0; i < fullDensity; i++) {
            float angle = i/ (fullDensity - 1f);

            if (useTorus) coil[i] = GetTorusCoilLocalPos(angle, fullDensity, i);
            else coil[i] = GetLinearCoilLocalPos(angle, fullDensity, i);
        }

        DrawCoil(coil);
    }

    void DrawCoil(Vector3[] coil) {
        for(int i = 0; i < coil.Length-1; i++) {
            //float percent = i / (coil.Length-1f);
            Vector3 posA = transform.TransformPoint(coil[i]);
            Vector3 posB = transform.TransformPoint(coil[i+1]);
            Debug.DrawLine(posA, posB, Color.Lerp(startColor, endColor, 0));
        }
    }

    Vector3 GetLinearCoilLocalPos(float angle, float density, int iterator) {
        return new Vector3(
            Mathf.Cos(angle * TAU * turns) * radius,
            Mathf.Sin(angle * TAU * turns) * radius,
            iterator * height / density
            );
    }

    Vector3 GetTorusCoilLocalPos(float angle, float density, int iterator) {
        float tRadius = torusCircumference / TAU;
        float tAngle = angle * TAU;
        Vector3 torusPos = new Vector3(Mathf.Cos(tAngle) * tRadius, 0f, Mathf.Sin(tAngle) * tRadius);
        Vector3 torusDir = torusPos.normalized;
        Vector3 torusUp = Vector3.up;

        Vector3 coilPos = new Vector3(
            Mathf.Cos(angle * TAU * turns) * radius,
            Mathf.Sin(angle * TAU * turns) * radius,
            0f
            );

        //Acabou sendo a mesma logica que fiz no coil só que escrito de forma diferente
        //Posição central do torus + direção up ou right * quantidade de sin/cos do coil
        return torusPos + coilPos.x * torusDir + coilPos.y * torusUp;
    }
}

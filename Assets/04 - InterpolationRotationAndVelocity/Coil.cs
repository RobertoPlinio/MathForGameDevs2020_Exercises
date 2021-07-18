using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : MonoBehaviour
{
    public bool a, b, c;

    [Header("9 A")]
    public int density_AB;
    public float numOfTurns_A;
    public float height;
    public float radius_A;

    [Header("9 B")]
    public Color startColor = Color.white;
    public Color endColor = Color.white;

    [Header("9 C")]
    public int density_Torus;
    public int density_C;
    public bool drawTorus = true;
    public float circumference;
    public int numOfTurns_B; //is an int to guarantee a loop
    public float radius_B;
    List<Vector3> torus = new List<Vector3>();

    const float TAU = Mathf.PI * 2;
    private void OnValidate() {
        if (c) a = b = false;
        else if (b) a = c = false;
        else if (a) b = c = false;

        density_AB = Mathf.Max(density_AB, 3);
    }

    private void Update() {
        if (a)
            DrawRegularCoil();
        if (b)
            DrawRegularCoil(true);
        if (c)
            DrawTorusCoil();
    }

    void DrawRegularCoil(bool lerpColors = false) {
        Vector3 previousPoint = Vector3.zero;
        Vector3 actualPoint = Vector3.zero;
        Vector3 relativeActual, relativePrevious;

        for (int i = 0; i <= density_AB; i++) {
            float percent = Mathf.InverseLerp(0, density_AB, i);
            float turnAmount = i * TAU / density_AB * numOfTurns_A;

            actualPoint.z = Mathf.Sin(turnAmount) * radius_A;
            actualPoint.x = Mathf.Cos(turnAmount) * radius_A;
            actualPoint.y = percent * height;

            relativePrevious = transform.InverseTransformPoint(previousPoint); //Just to make the code easier to read
            relativeActual = transform.InverseTransformPoint(actualPoint);

            if (i > 0) {
                if (!lerpColors)
                    Debug.DrawLine(relativePrevious, relativeActual, Color.white);
                else
                    Debug.DrawLine(relativePrevious, relativeActual, Color.Lerp(startColor, endColor, percent));
            }

            previousPoint = actualPoint;
        }
    }

    void DrawTorusCoil() {
        Vector3 previousPoint = Vector3.zero;
        Vector3 actualPoint = Vector3.zero;
        Vector3 relativeActual, relativePrevious;
        float percent;
        float turnAmount;

        torus.Clear();

        //TORUS
        for (int i = 0; i <= density_Torus; i++) {
            turnAmount = i * TAU / density_Torus;

            float radius = circumference / TAU;
            actualPoint.z = Mathf.Sin(turnAmount) * radius;
            actualPoint.x = Mathf.Cos(turnAmount) * radius;

            relativePrevious = transform.InverseTransformPoint(previousPoint);
            relativeActual = transform.InverseTransformPoint(actualPoint);

            if (drawTorus && i > 0) Debug.DrawLine(relativePrevious, relativeActual, Color.blue);

            torus.Add(relativeActual);
            previousPoint = actualPoint;
        }

        //COIL
        for (int i = 0; i <= density_C; i++) {
            percent = Mathf.InverseLerp(0, density_C, i);
            turnAmount = i * TAU / density_C * numOfTurns_B;

            actualPoint = FindPointInTorus(percent);

            Vector3 torusCenterRight = (actualPoint - transform.position).normalized;
            Vector3 torusCenterUp = transform.up;

            actualPoint += torusCenterRight * Mathf.Sin(turnAmount) * radius_B;
            actualPoint += torusCenterUp * Mathf.Cos(turnAmount) * radius_B;

            if (i > 0) Debug.DrawLine(previousPoint, actualPoint, Color.white);

            previousPoint = actualPoint;
        }
    }

    Vector3 FindPointInTorus(float percent) {
        float indexF = Mathf.Lerp(0, torus.Count - 1, percent);
        int index = (int)indexF;

        Vector3 ptA = torus[index];
        Vector3 ptB = index < (torus.Count - 1) ? torus[index+1] : ptA;

        return Vector3.Lerp(ptA, ptB, indexF - index);
    }
}

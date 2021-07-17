using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class RegularPolygonDrawer : MonoBehaviour
{
    [Range(0, 15)]
    public int vertices;
    public float vertexRadius = 0.05f;
    public Color vertexColor = Color.white;
    [Space]
    [Range(0, 5)]
    public int jumpVert = 0;
    [Space]
    public bool drawCircle = false;
    public bool showLabel = false;
    public bool showMesh = false;
    public int meshLines = 10;

    public List<Vector3> polygonVertices;

    public GUIStyle guiStyle;

    private void OnDrawGizmos() {
        if (vertices <= 2) return;

        //The way it works is by creating a unit circle a using
        // one turn of this circle as the path the vertices will
        // be put on. One turn = 1 radian (2 Pi or 1 Tau)
        //Vertices will be placed in a % of the turn and vertices amount

        if (drawCircle) {
            Gizmos.color = new Color(1, 1, 1, 0.2f);
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        polygonVertices = new List<Vector3>();

        //Get vertices points on unit circle (radius = 1/2)
        for (int i = 0; i < vertices; i++) {
            Vector3 vertPos = transform.position;
            vertPos.x += Mathf.Cos(i * 2 * Mathf.PI / vertices) * 0.5f; //0.5 is the radius of the unit circle
            vertPos.y += Mathf.Sin(i * 2 * Mathf.PI / vertices) * 0.5f;

            polygonVertices.Add(vertPos);
        }

        //Draw verts and connect one another
        for (int i = 0; i < polygonVertices.Count; i++) {
            Gizmos.color = vertexColor;
            Gizmos.DrawSphere(polygonVertices[i], vertexRadius);

            if (showLabel) Handles.Label((transform.position + (polygonVertices[i] - transform.position)), $"{i}");

            int connectVert = ((i + 1) + jumpVert) % polygonVertices.Count;
            Gizmos.DrawLine(polygonVertices[i], polygonVertices[connectVert]);
        }

        if (showMesh) {
            //Um pequeno migue porque tudo que tentei criando mesh no on draw gizmo nao funcionou
            // e nao quero que esse codigo precise do play mode para funcionar
            for (int i = 0; i < polygonVertices.Count; i ++) {
                float l = 0;
                while (l <= 1) {
                    Gizmos.DrawLine(Vector3.Lerp(polygonVertices[i], polygonVertices[((i + 1) + jumpVert) % polygonVertices.Count], l), transform.position);
                    l += 1f/ meshLines;
                }
            }
        }

        Handles.Label(transform.position + transform.up * 0.75f, $"Vertices = {vertices}\nDensity = {jumpVert + 1}", guiStyle);
    }
}

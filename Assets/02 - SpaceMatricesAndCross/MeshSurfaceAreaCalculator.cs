using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSurfaceAreaCalculator : MonoBehaviour
{
    public MeshFilter meshF;
    Mesh mesh => meshF.sharedMesh;

    public bool countSurfaceArea;
    public float surficeArea;

    private void OnDrawGizmos() {
        if (countSurfaceArea && mesh) {
            surficeArea = 0f;
            countSurfaceArea = false;

            Vector3[] verts = mesh.vertices;
            int[] tris = mesh.triangles;

            Debug.DrawLine(meshF.transform.position + verts[tris[0]], meshF.transform.position + verts[tris[1]], Color.red);
            Debug.DrawLine(meshF.transform.position + verts[tris[1]], meshF.transform.position + verts[tris[2]], Color.cyan);
            Debug.DrawLine(meshF.transform.position + verts[tris[2]], meshF.transform.position + verts[tris[0]], Color.green);

            Vector3 meshPos = meshF.transform.position;
            for(int i = 0; i< mesh.triangles.Length; i+=3) {

                Vector3 ab = verts[tris[i + 1]] - verts[tris[i]];
                Vector3 ac = verts[tris[i + 2]] - verts[tris[i]];

                Debug.DrawLine(meshPos + verts[tris[i]], meshPos + verts[tris[i + 1]], Color.red);
                Debug.DrawLine(meshPos + verts[tris[i + 1]], meshPos + verts[tris[i + 2]], Color.cyan);
                Debug.DrawLine(meshPos + verts[tris[i + 2]], meshPos + verts[tris[i]], Color.green);

                surficeArea += Vector3.Cross(ab, ac).magnitude / 2;
            }
        }
    }
}

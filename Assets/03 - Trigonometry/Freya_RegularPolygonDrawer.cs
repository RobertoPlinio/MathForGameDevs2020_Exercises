using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freya_RegularPolygonDrawer : MonoBehaviour
{
    //A grande diferença observada entre meu metodo e o da Freya foi a forma mais limpa que a função separada
    // de obter o angulo deixa o codigo. Porem a versao dela fica fixada no zero da cena. Eh possivel que 
    // para seguir o game object seja preciso uma modificacao que deixe o codigo dela mais parecido com o meu

    [Range(4f, 12f)]
    public int sideCount = 4;

    [Range(1f, 5f)]
    public int density = 1;

    const float TAU = 6.28318530718f;
    Vector2 AngToDir(float angRad) => new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
    float DirToAng(Vector2 v) => Mathf.Atan2(v.y, v.x);

    private void OnDrawGizmos() {
        //generating data
        Vector2[] verts = new Vector2[sideCount];

        for (int i = 0; i < sideCount; i++) {
            verts[i] = AngToDir(i * TAU / sideCount);
        }

        //Drawing
        Gizmos.color = Color.white;
        for (int i = 0; i < sideCount; i++) {
            Gizmos.DrawLine(verts[i], verts[(i + density) % sideCount]);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < sideCount; i++) {
            Gizmos.DrawSphere(verts[i], 0.05f);
        }
    }
}

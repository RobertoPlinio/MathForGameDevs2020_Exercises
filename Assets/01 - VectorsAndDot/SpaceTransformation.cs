using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTransformation : MonoBehaviour
{
    public Transform target;
    public Vector3 localSpace, worldSpace;

    private void OnDrawGizmos() {
        if (!target) return;

        // Dado um world space, para transformar em local em relação a esse objeto
        // Faço um vetor entre eu e o world space
        Vector3 originToTarget = target.position - transform.position;

        // Os valores do Dot nos meus eixos locais X e Y (right e up, length 1) vão me dar as diferenças nas posições
        // que logo é a posição do local desse world space em relação a mim, já que o vetor não é normalizado
        float yDot = Vector3.Dot(transform.up, originToTarget);
        float xDot = Vector3.Dot(transform.right, originToTarget);

        localSpace = new Vector3(xDot, yDot, 0);

        // Dado um local space, para transformar em world space
        // Uso os valores de local space somando minha posição (que já é em global), no meus eixos
        Vector3 MePlusLocal = transform.position + (transform.up * localSpace.y) + (transform.right * localSpace.x);

        worldSpace = MePlusLocal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory_A : MonoBehaviour
{
    //I will assume the reference game object this inventory
    // will show arcs is the same this script is on

    public int numberOfItems;
    public float arcRadius;
    public float itemRadius;

    private void OnDrawGizmos() {
        if (numberOfItems < 1) return;

        float itemDifferenceAngle = Mathf.Asin(itemRadius / arcRadius) * 2;
        //The offset to be subtracted as to make the items in the center
        float angleOffset = itemDifferenceAngle * (numberOfItems - 1) / 2f;

        for (int i = 0; i < numberOfItems; i++) {
            float angle = i * itemDifferenceAngle - angleOffset;
            Vector3 itemPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * arcRadius;
            Handles.DrawWireDisc(itemPos, transform.forward, itemRadius);
        }
    }
}

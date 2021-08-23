using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Inventory_B : MonoBehaviour
{
    public float arcRadius;

    public Item[] items;

    private void OnDrawGizmos() {
        float numberOfItems = items.Length;
        if (numberOfItems < 1) return;

        float itemDifferenceAngle = 0;
        float angleOffset = 0;

        for (int i = 0; i < numberOfItems; i++) {
            itemDifferenceAngle = Mathf.Asin(items[i].radius / arcRadius);
            if (i + 1 < numberOfItems) itemDifferenceAngle += Mathf.Asin(items[i + 1].radius / arcRadius);
            angleOffset += itemDifferenceAngle;
        }

        angleOffset = angleOffset * (numberOfItems - 1) / 2f;

        for (int i = 0; i < numberOfItems; i++) {
            itemDifferenceAngle = Mathf.Asin(items[i].radius / arcRadius) * 2f;
            float angle = i * itemDifferenceAngle - angleOffset;
            Vector3 itemPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * arcRadius;
            Handles.DrawWireDisc(itemPos, transform.forward, items[i].radius);
        }
    }

    [Serializable]
    public struct Item
    {
        public float radius;
    }
}

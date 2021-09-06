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

        int itemMax = items.Length;
        float angleDelta = 0;
        float angleOffset = 0;
        float a;
        float bc = 2 * arcRadius * arcRadius;

        for(int i = 1; i < itemMax; i++) {
            //Law of cosines - Solution of trianges (three given sides SSS)
            a = Mathf.Pow(items[i].radius + items[i - 1].radius, 2);
            items[i].delta = (Mathf.Acos(1 - (a / bc)));
        }

        for (int i = 0; i < itemMax; i++) angleOffset += items[i].delta;
        angleOffset /= 2f;

        Vector3 itemPos = new Vector3(Mathf.Cos(-angleOffset), Mathf.Sin(-angleOffset)) * arcRadius;
        Handles.DrawWireDisc(transform.TransformPoint(itemPos), transform.forward, items[0].radius);
        for (int i = 1; i < itemMax; i++) {
            angleDelta += items[i].delta;
            itemPos = new Vector3(Mathf.Cos(angleDelta - angleOffset), Mathf.Sin(angleDelta - angleOffset)) * arcRadius;
            Handles.DrawWireDisc(transform.TransformPoint(itemPos), transform.forward, items[i].radius);
        }
    }

    [Serializable]
    public struct Item
    {
        [Range(0, 1)] public float radius;
        [NonSerialized] public float delta;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util : MonoBehaviour{

    public static Vector2 lerp(GameObject[] points, float u, int i0 = 0, int i1 = -1) {
        Vector2[] vpoints = new Vector2[points.Length];
        for (int i = 0; i < points.Length; ++i) {
            vpoints[i] = points[i].transform.position;
        }
        return lerp(vpoints, u, i0, i1);
    }

    public static Vector2 lerp(Vector2[] points, float u, int i0 = 0, int i1 = -1) {
        if (i1 == -1) {
            i1 = points.Length - 1;
        }
        if (i0 == i1) {
            return points[i0];
        }

        Vector2 point1 = lerp(points, u, i0, i1 - 1);
        Vector2 point2 = lerp(points, u, i0 + 1, i1);
        Vector2 point12 = (1 - u) * point1 + u * point2;
        return point12;
    }
}

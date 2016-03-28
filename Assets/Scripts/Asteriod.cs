using UnityEngine;
using System;
using System.Collections;

public class Asteriod : MonoBehaviour {
    public float move_speed;
    public float max_lerp_offset;

    Vector2[] lerp_points = new Vector2[2];
    float lerp_time;
    float lerp_start;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < lerp_points.Length; ++i) {
            lerp_points[i] = new Vector2(
                transform.position.x + UnityEngine.Random.Range(0f, max_lerp_offset),
                transform.position.y + UnityEngine.Random.Range(0f, max_lerp_offset));
        }
        lerp_time = Vector2.Distance(lerp_points[0], lerp_points[1]) / move_speed;
        lerp_start = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    float u = (Time.time - lerp_start) / lerp_time;
        if (u > 1f) {
            u -= 1f;
            lerp_start = Time.time;
            Array.Reverse(lerp_points);
        }
        this.transform.position = Util.lerp(lerp_points, u);
	}
}

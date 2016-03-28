using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {
    public float lerp_time;
    public GameObject[] lerp_points;

    float lerp_start;

	// Use this for initialization
	void Start () {
        lerp_start = 0f;	
	}
	
	// Update is called once per frame
	void Update () {
        float u = (Time.time - lerp_start) / lerp_time;
        if (u > 1f) {
            u = 0f;
            lerp_start = Time.time;
            Array.Reverse(lerp_points);
        }
        this.transform.position = Util.lerp(lerp_points, u);
	}
}

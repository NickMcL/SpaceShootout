using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Asteriod : MonoBehaviour {
    public List<GameObject> asteroid_list;

    public float move_speed;
    public float min_lerp_offset;
    public float max_lerp_offset;

    Vector3[] lerp_points = new Vector3[2];
    float lerp_time;
    float lerp_start;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < lerp_points.Length; ++i) {
            lerp_points[i] = new Vector3(
                transform.position.x + UnityEngine.Random.Range(min_lerp_offset, max_lerp_offset),
                transform.position.y + UnityEngine.Random.Range(min_lerp_offset, max_lerp_offset), 0f);
        }
        lerp_time = Vector3.Distance(lerp_points[0], lerp_points[1]) / move_speed;
        lerp_start = 0;
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

    public void Destroy()
    {
        CameraShaker.S.DoShake(.1f, 0f);
        foreach (GameObject obj in asteroid_list)
        {
            GameObject astroid = Instantiate(obj, UnityEngine.Random.insideUnitCircle  + new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        }
        if (asteroid_list.Count != 0)
        {
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Asteriod : MonoBehaviour {
    public List<GameObject> asteroid_list;
    public List<Sprite> sprite_list;
    public GameObject explosionPrefab;

    public bool doLerp = true;
    public float launchSpeed;

    public float move_speed;
    public float min_lerp_offset;
    public float max_lerp_offset;

    Vector3[] lerp_points = new Vector3[2];
    float lerp_time;
    float lerp_start;

    int hitcount;

    public GameObject ball;
    

	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("Ball");
        for (int i = 0; i < lerp_points.Length; ++i)
        {
            lerp_points[i] = new Vector3(
                transform.position.x + UnityEngine.Random.Range(min_lerp_offset, max_lerp_offset),
                transform.position.y + UnityEngine.Random.Range(min_lerp_offset, max_lerp_offset), 0f);
        }
        if(lerp_points.Length > 1) lerp_time = Vector3.Distance(lerp_points[0], lerp_points[1]) / move_speed;
        lerp_start = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (doLerp)
        {
            float u = (Time.time - lerp_start) / lerp_time;
            if (u > 1f)
            {
                u = 0f;
                lerp_start = Time.time;
                Array.Reverse(lerp_points);
            }
            this.transform.position = Util.lerp(lerp_points, u);
        }
	}

    public void Destroy()
    {
        if (!ball.GetComponent<SoccerBall>().ball_in_play) return;
        CameraShaker.S.DoShake(.1f, 0f);

        hitcount++;
        if (hitcount < 2)
        {
            GetComponent<SpriteRenderer>().sprite = sprite_list[hitcount];
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;

        foreach (GameObject obj in asteroid_list)
        {
            GameObject astroid = Instantiate(obj, UnityEngine.Random.insideUnitCircle  + new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
            Rigidbody2D rb2d = astroid.AddComponent<Rigidbody2D>();

            rb2d.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * launchSpeed, ForceMode2D.Impulse);
            rb2d.gravityScale = 0f;
            rb2d.mass = 4f;
            astroid.GetComponent<Asteriod>().doLerp = false;
        }
        if (asteroid_list.Count != 0)
        {
            //Replay.replay_device.killObject(gameObject);
            Destroy(this.gameObject);
        }
    }
}

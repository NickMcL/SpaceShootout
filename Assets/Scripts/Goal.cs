using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {
    public static Goal S;
    public HUD.Team team;

    public float lerp_time;
    public GameObject[] lerp_points;

    float lerp_start;
    Vector3 last_pos;
    GameObject[] original_point_order;

    public GameObject lookat;
    public Vector3 targetPoint;
    public Quaternion targetRotation;
    bool forward = true;

    public GameObject explosionPrefab;
    public Collider2D[] inRange;
    public float explodeRadius = 4f;
    public float explodeForce = 1000f;

    void Awake() {
        S = this;
    }

    // Use this for initialization
    void Start() {
        lerp_start = 0f;
        original_point_order = new GameObject[lerp_points.Length];
        Array.Copy(lerp_points, original_point_order, lerp_points.Length);
        resetGoal();
    }

    // Update is called once per frame
    void Update() {
        if (lookat != null) {
            Vector2 dirToTarget = lookat.transform.position - transform.position;
            if (forward) {
                transform.Rotate(Vector3.forward, Vector2.Angle(dirToTarget, transform.up));
            } else {
                transform.Rotate(-Vector3.forward, Vector2.Angle(dirToTarget, transform.up));
            }
        }
        if (!HUD.S.GameStarted) {
            this.transform.position = last_pos;
            lerp_start = Time.time;
            return;
        }
        float u = (Time.time - lerp_start) / lerp_time;
        if (u > 1f) {
            u = 0f;
            lerp_start = Time.time;
            Array.Reverse(lerp_points);
            forward = !forward;
        }
        last_pos = Util.lerp(lerp_points, u);
        this.transform.position = last_pos;


    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ball" && HUD.S.GameStarted) {

            //set to true for replay
            if (false) {
              //  Replay.replay_device.startPlayback(Replay.replay_device.current_time - 500, Replay.replay_device.current_time, 1);
                return;
            }
            HUD.S.PlaySound("explosion", 1);

            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            explodeAtPoint();
            Global.S.score(team);

        }
    }

    public void resetGoal() {
        Array.Copy(original_point_order, lerp_points, lerp_points.Length);
        last_pos = Util.lerp(lerp_points, 0f);
        this.transform.position = Util.lerp(lerp_points, 0f);
    }

    public void explodeAtPoint()
    {
        inRange = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
        foreach (Collider2D collider in inRange)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (collider.GetComponent<Rigidbody2D>() != null)
                {
                    float x = collider.transform.position.x - transform.position.x;
                    float y = collider.transform.position.y - transform.position.y;
                    collider.GetComponent<Rigidbody2D>().AddForce((new Vector2(x, y))*explodeForce, ForceMode2D.Force);
                }
            }
        }
    }
}

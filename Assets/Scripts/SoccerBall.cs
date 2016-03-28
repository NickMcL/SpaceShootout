using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour {
    Rigidbody2D rb;
    Rigidbody ballrb;
    float max_speed = 50;
    public static GameObject Ball;
    Vector3 WayToGo;

    // Use this for initialization
    void Awake() {
        Ball = gameObject;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ballrb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        WayToGo.x = rb.velocity.y;
        WayToGo.y = -rb.velocity.x;
        ballrb.angularVelocity = WayToGo;
    }

    // Update is called once per frame
    void Update() {
        while (rb.velocity.magnitude > max_speed) {
            rb.velocity *= 0.99f;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player" && transform.parent != null &&
                    coll.gameObject.GetComponent<Player>().is_goalie) {
            transform.parent.gameObject.GetComponent<Player>().loseControlOfBall();
        }
    }
}

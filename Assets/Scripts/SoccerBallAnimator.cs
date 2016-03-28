using UnityEngine;
using System.Collections;

public class SoccerBallAnimator : MonoBehaviour {
    Rigidbody2D rb;
    Rigidbody ballrb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        ballrb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
	}

    Vector3 WayToGo;

    void FixedUpdate()
    {
        WayToGo.x = rb.velocity.y;
        WayToGo.y =- rb.velocity.x;
        ballrb.angularVelocity = WayToGo;
    }

	// Update is called once per frame
	void Update () {
	
	}
}

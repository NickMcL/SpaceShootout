using UnityEngine;
using System.Collections;

public class BallCannotOverlap : MonoBehaviour {

    int ball_collider_index;

	// Use this for initialization
	void Start () {
        //Player.ball_check_colliders.Add(GetComponent<Collider2D>());
        //ball_collider_index = Player.ball_check_colliders.Count - 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {
        //Player.ball_check_colliders.RemoveAt(ball_collider_index);
    }
}

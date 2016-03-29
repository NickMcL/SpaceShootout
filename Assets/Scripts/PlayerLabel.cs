using UnityEngine;
using System.Collections;

public class PlayerLabel : MonoBehaviour {

    public Vector3 offset = new Vector3(0f, 0.25f, 0f);
    public GameObject follow_player;

	// Use this for initialization
	void Start () {
    	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = follow_player.transform.position + offset;	
	}
}

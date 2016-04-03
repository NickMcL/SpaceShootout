using UnityEngine;
using System.Collections;

public class Replayable : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // prepForReplay();
	}
    void prepForReplay() {
        /*
        if (gameObject.name.Contains("Clone")) {
           
            Destroy(this);
            return;
        }
        */
        //Replay.replay_device.addToList(gameObject);
    }
    void FixedUpdate() {
       // Replay.replay_device.updateObject(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControllerArea : MonoBehaviour {
    const int TOTAL_PLAYERS = 4;
    static int players_set = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (players_set == TOTAL_PLAYERS) {
            SceneManager.LoadScene("CharacterSelect");
        }
	}

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "ControllerSelector") {
            ControllerSelector selector = coll.gameObject.GetComponent<ControllerSelector>();
            ControlManager.setPlayerDevice(selector.player_num, selector.device);
            ++players_set;
            Destroy(coll.gameObject);
        }
    }
}

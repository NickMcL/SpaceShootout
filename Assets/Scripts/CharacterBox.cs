using UnityEngine;
using System.Collections;

public class CharacterBox : MonoBehaviour {

    public string character_name;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag != "PlayerSelector") {
            return;
        }

        PlayerSelector player = coll.gameObject.transform.parent.gameObject.GetComponent<PlayerSelector>();
        player.in_box = true;
        TitleScreen.S.hoverOverCharacter(player.player_num, character_name);
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag != "PlayerSelector") {
            return;
        }

        PlayerSelector player = coll.gameObject.transform.parent.gameObject.GetComponent<PlayerSelector>();
        player.in_box = false;
        TitleScreen.S.stopHoverOverCharacter(player.player_num);
    }
}

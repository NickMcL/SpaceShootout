using UnityEngine;
using System.Collections;

public class PlayerSelector : MonoBehaviour {

    public int player_num = 0;
    public float move_speed = 5f;
    public float rumble_time = 1f;
    public Sprite red_pointer;
    public Sprite blue_pointer;
    public Vector2 start_position;

    Rigidbody2D rb;
    SpriteRenderer sprite_renderer;
    bool finished_selecting;
    public bool in_box;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        finished_selecting = false;
        in_box = false;
        transform.position = start_position;

        if (player_num == 0 || player_num == 1) {
            sprite_renderer.sprite = blue_pointer;
        } else {
            sprite_renderer.sprite = red_pointer;
        }
        transform.FindChild("PlayerNum").gameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>("p" + (player_num + 1));
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = ControlManager.getMovementVector(player_num).normalized * move_speed;

        if (ControlManager.confirmationButtonPressed(player_num) && !finished_selecting && in_box) {
            TitleScreen.S.chooseCharacter(player_num);
            hidePointer();
            finished_selecting = true;
            ControlManager.rumble(player_num);
            Invoke("stopRumble", rumble_time);
        }

        if (ControlManager.undoButtonPressed(player_num) && finished_selecting) {
            TitleScreen.S.unchooseCharacter(player_num);
            showPointer();
            finished_selecting = false;
        }
	}

    void hidePointer() {
        SpriteRenderer child_sr;
        setAlpha(sprite_renderer, 0);

        foreach (Transform child in transform) {
            child_sr = child.GetComponent<SpriteRenderer>();
            if (child_sr != null) {
                setAlpha(child_sr, 0);
            }
        }
    }

    void showPointer() {
        SpriteRenderer child_sr;
        setAlpha(sprite_renderer, 1);
        transform.position = start_position;

        foreach (Transform child in transform) {
            child_sr = child.GetComponent<SpriteRenderer>();
            if (child_sr != null) {
                setAlpha(child_sr, 1);
            }
        }
    }

    void setAlpha(SpriteRenderer sr, float alpha) {
        Color new_color = sr.color;
        new_color.a = alpha;
        sr.color = new_color;
    }

    void stopRumble() {
        ControlManager.rumble(player_num, true);
    }
}

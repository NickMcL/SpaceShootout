using UnityEngine;
using System.Collections;
using InControl;

public class ControllerSelector : MonoBehaviour {
    public float velocity = 7f;
    public int player_num;
    public InputDevice device;

    Rigidbody2D rigid;

    void Awake() {
        device = InputManager.Devices[player_num];
    }

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 movement_vector = device.LeftStick;
        rigid.velocity = movement_vector.normalized * velocity;
	}
}

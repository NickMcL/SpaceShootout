using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class ControlManager : MonoBehaviour {
    const int TOTAL_PLAYERS = 4;
    
    static Dictionary<string, List<KeyCode>> player_key_map = new Dictionary<string, List<KeyCode>>() {
        { "up", new List<KeyCode>() { KeyCode.UpArrow, KeyCode.W, KeyCode.T, KeyCode.I} },
        { "down", new List<KeyCode>() { KeyCode.DownArrow, KeyCode.S, KeyCode.G, KeyCode.K} },
        { "left", new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.A, KeyCode.F, KeyCode.J} },
        { "right", new List<KeyCode>() { KeyCode.RightArrow, KeyCode.D, KeyCode.H, KeyCode.L} },
        { "boost", new List<KeyCode>() { KeyCode.Keypad0, KeyCode.X, KeyCode.B, KeyCode.Comma} },
        { "fire", new List<KeyCode>() { KeyCode.KeypadPeriod, KeyCode.C, KeyCode.N, KeyCode.Period} },
        { "special", new List<KeyCode>() { KeyCode.KeypadEnter, KeyCode.V, KeyCode.M, KeyCode.Question} }
    };

    public static bool use_controllers = false;
    static InputDevice[] players = new InputDevice[TOTAL_PLAYERS];

    static public void setPlayerDevice(int player_num, InputDevice device) {
        players[player_num] = device;
    }

    static Vector2 getKeyboardMovement(int player_num) {
        Vector2 move_vector = Vector2.zero;
        if (Input.GetKey(player_key_map["up"][player_num])) {
            move_vector.y += 1;
        } else if (Input.GetKey(player_key_map["down"][player_num])) {
            move_vector.y -= 1;
        } else if (Input.GetKey(player_key_map["right"][player_num])) {
            move_vector.x += 1;
        } else if (Input.GetKey(player_key_map["left"][player_num])) {
            move_vector.x -= 1;
        }
        return move_vector;
    }

    static public Vector2 getMovementVector(int player_num) {
        if (use_controllers) {
            return players[player_num].LeftStick;
        }
        return getKeyboardMovement(player_num);
    }

    static public Vector2 getDribbleVector(int player_num) {
        if (use_controllers) {
            return players[player_num].RightStick;
        }
        return getKeyboardMovement(player_num);
    }

    static public bool boostButtonPressed(int player_num) {
        if (use_controllers) {
            return players[player_num].LeftTrigger.IsPressed;
        }
        return Input.GetKey(player_key_map["boost"][player_num]);
    }

    static public bool fireButtonPressed(int player_num) {
        if (use_controllers) {
            return players[player_num].RightTrigger.IsPressed;
        }
        return Input.GetKey(player_key_map["fire"][player_num]);
    }

    static public bool specialButtonPressed(int player_num) {
        if (use_controllers) {
            return players[player_num].Action1.IsPressed;
        }
        return Input.GetKey(player_key_map["special"][player_num]);
    }

    static public void rumble(int player_num, bool stop = false) {
        if (use_controllers) {
            if (stop) {
                players[player_num].Vibrate(0f);
            } else {
                players[player_num].Vibrate(0.4f);
            }
        }
    }
}

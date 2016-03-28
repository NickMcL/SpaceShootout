using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    struct controls {
        public string up, vert, hor;
        public string skate, Shoot, special;
    };
    controls my_inputs;
    public int my_number = 1;
    Rigidbody2D rigid;
    float max_speed = 5;
    float acceleration = 30;
    float dash_delay_time = 0.3f;
    float dash_delay = 0;
    float slow_delay_time = 1f;
    float slow_delay = 0;
    int slowed = 0;
    float fatigue = 0;
    bool dash = false;
    public bool has_ball = false;
    public bool is_goalie = false;
    GameObject ball;
    Rigidbody2D ball_rb;

    void Start() {
        ball = SoccerBall.Ball;
        ball_rb = ball.GetComponent<Rigidbody2D>();
        my_inputs.vert = string.Format("Vertical{0}", my_number);
        my_inputs.hor = string.Format("Horizontal{0}", my_number);
        my_inputs.skate = string.Format("Skate{0}", my_number);
        my_inputs.Shoot = string.Format("Shoot{0}", my_number);
        my_inputs.special = string.Format("Special{0}", my_number);
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        checkDash();
        updateMovement();
        if (has_ball == true) {
            dribble();
        }
        if (getInputFire()) {
            shoot();
        }

    }

    void updateMovement() {
        Vector2 move_vector = getInputMovementVector();
        move_vector *= acceleration;

        if (dash == true) {
            move_vector *= 20;
            dash = false;
        }/*
        if (slowed >0) {
            rigid.velocity = rigid.velocity/(1+slowed*0.2f);
        }
          */
        rigid.AddForce(move_vector / (1 + slowed * 0.75f));
        if (rigid.velocity.magnitude > max_speed && dash_delay < dash_delay_time * 0.8f) {
            rigid.velocity *= 0.97f;
        }

    }


    Vector2 getInputMovementVector() {
        Vector2 move_vector = Vector2.zero;
        move_vector.x += Input.GetAxis(my_inputs.hor);
        move_vector.y += Input.GetAxis(my_inputs.vert);

        return move_vector;
    }

    bool getInputFire() {
        return Input.GetAxis(my_inputs.Shoot) > 0;

    }

    void checkDash() {
        if (slow_delay > 0) {
            slow_delay -= Time.deltaTime;
        } else if (slowed > 0) {
            slowed--;
            if (slowed > 0) {
                slow_delay = slow_delay_time;
            }
        }

        if (Input.GetAxis(my_inputs.skate) > 0 && dash_delay <= 0 && slowed < 3) {
            dash = true;
            dash_delay = dash_delay_time;
        } else if (dash_delay > 0) {
            dash_delay -= Time.deltaTime;
            if (dash_delay <= 0) {
                slowed++;
                slow_delay = slow_delay_time;
            }
        }
    }
    void dribble() {
        Rigidbody2D ball_rb = ball.GetComponent<Rigidbody2D>();



    }
    void shoot() {
        Vector2 shot = ball.transform.position-transform.position;
        Vector3.Normalize(shot);
        shot *= 10f;
        ball_rb.AddForce(shot);
    }
    bool getInputPower() {
        return Input.GetAxis(my_inputs.special) > 0;
    }

}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    float DRIBBLE_MAGNITUDE_THRESHOLD = 0.8f;
    Color PLAYER_1_COLOR = new Color(1f, 0.7f, 0.7f);
    Color PLAYER_2_COLOR = new Color(0.7f, 0.7f, 1f);

    public int my_number = 1;
    public float ball_offset_scale;
    public bool has_ball = false;
    public HUD.Team team;

    float max_speed = 5;
    public float acceleration = 30;
    float dash_delay_time = 0.3f;
    float dash_delay = 0;
    float slow_delay_time = 1f;
    float slow_delay = 0;
    public float shot_force = 1000f;
    public float charge_shot_delay = 1.5f;
    public float charge_shot_multiplier = 2f;
    public int charged_emit = 10;
    public float charged_speed = 10f;
    Vector2 current_ball_angle;

    int slowed = 0;
    float fatigue = 0;
    bool dash = false;
    bool shooting = false;
    float shot_start_time;

    GameObject ball;
    Rigidbody2D ball_rb;
    Rigidbody2D rigid;
    CircleCollider2D player_collider;
    CircleCollider2D ball_collider;
    Color default_color;
    public GameObject label;

    // should be the first thind done in anything that needs to be in a replay
   

    void Start() {
        ball = SoccerBall.Ball;
        ball_rb = ball.GetComponent<Rigidbody2D>();
        player_collider = GetComponent<CircleCollider2D>();
        ball_collider = ball.GetComponent<CircleCollider2D>();
        rigid = gameObject.GetComponent<Rigidbody2D>();

        if (my_number == 0 || my_number == 1) {
            team = HUD.Team.BLUE;
        } else {
            team = HUD.Team.RED;
        }

        default_color = GetComponent<Renderer>().material.color;
        label.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("p" + (my_number+1));
    }

    void FixedUpdate() {
  //      Replay.replay_device.updateObject(gameObject);
    }

    void Update() {
        if (!HUD.S.GameStarted) {
            return;
        }
        checkDash();
        updateMovement();
        /*
        if (slowed == 0) {
            GetComponent<Renderer>().material.color = default_color;
        } else if(slowed == 1) {
            GetComponent<Renderer>().material.color = Color.yellow;
        } else if (slowed == 2) {
            GetComponent<Renderer>().material.color = Color.yellow/2+Color.red/2;
        }else if(slowed == 3) {
            GetComponent<Renderer>().material.color = Color.red;
        }
        */

        if (has_ball == true) {
            dribble();
        }
        if (ControlManager.fireButtonPressed(my_number) && has_ball && !shooting) {
            startShot();
        }
        if (shooting && has_ball) {
            finishShot();
        }
    }

    void updateMovement() {
        Vector2 move_vector = ControlManager.getMovementVector(my_number);
        move_vector *= acceleration;

        if (dash == true) {
            HUD.S.PlaySound("woosh", Random.Range(.5f, 1f));
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

    public void gainControlOfBall() {
        HUD.S.PlaySound("dribble", Random.Range(.5f, 1f));
        ball.transform.SetParent(transform);
        has_ball = true;
        current_ball_angle = (ball.transform.position - transform.position).normalized;
        dribble();
        ball_rb.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(player_collider, ball_collider, true);
    }

    public void loseControlOfBall() {
        if (ball.transform.parent == transform) {
            HUD.S.stopLaserCharge();
            ControlManager.rumble(my_number, true);
            ball.transform.parent = null;
            has_ball = false;
            shooting = false;
            if (HUD.S.GameStarted) {
                Invoke("allowBallCollision", 0.5f);
            } else {
                Physics2D.IgnoreCollision(player_collider, ball_collider, false);
            }
        }
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

        if (ControlManager.boostButtonPressed(my_number) && dash_delay <= 0 && slowed < 3) {
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
        Vector2 dribble_vector = ControlManager.getDribbleVector(my_number);
        if (dribble_vector.magnitude > DRIBBLE_MAGNITUDE_THRESHOLD) {
            current_ball_angle = dribble_vector.normalized;
        }
        if (current_ball_angle != Vector2.zero) {
            ball.transform.localPosition = current_ball_angle * ball_offset_scale;
        }
    }

    void startShot() {
        shooting = true;
        shot_start_time = Time.time;
        HUD.S.startLaserCharge();
    }

    void finishShot() {
        if (!ControlManager.fireButtonPressed(my_number) && shooting) {
            actuallyShoot();
        } else {
            float charge_time = Time.time - shot_start_time;
     
            while ((charge_time / 2) - 1 > slowed) {
                if (slowed < (max_speed / 2)) {
                    slowed++;
                }
            }
            
            if (charge_time > charge_shot_delay) {
                ControlManager.rumble(my_number);
                ball.GetComponent<ParticleSystem>().Emit(charged_emit/2);
                ball.GetComponent<ParticleSystem>().startSpeed = charged_speed;
            } else {
                ball.GetComponent<ParticleSystem>().Emit((int)(charge_time/charge_shot_delay * charged_emit/2));
                ball.GetComponent<ParticleSystem>().startSpeed = (int)(charged_speed * charge_time / charge_shot_delay);
            }
        }
    }

    void actuallyShoot() {
        HUD.S.PlaySound("kick", Random.Range(.5f, 1f));
        loseControlOfBall();
        Vector2 shot = ball.transform.position - transform.position;
        Vector3.Normalize(shot);
        shot *= shot_force;
        if (Time.time - shot_start_time > charge_shot_delay) {
            shot *= charge_shot_multiplier;
            ball.GetComponent<SoccerBall>().fadeParticles(charged_emit);
            HUD.S.fireLaser();
        }
        ball_rb.AddForce(shot);
        has_ball = false;
        shooting = false;
    }

    void allowBallCollision() {
        Physics2D.IgnoreCollision(player_collider, ball_collider, false);
    }

    public void burstRumble(float duration) {
        ControlManager.rumble(my_number);
        Invoke("endRumble", duration);
    }

    void endRumble() {
        ControlManager.rumble(my_number, true);
    }
}

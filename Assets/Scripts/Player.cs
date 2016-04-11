using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    float DRIBBLE_MAGNITUDE_THRESHOLD = 0.8f;
    Color PLAYER_1_COLOR = new Color(1f, 0.8f, 0.8f);
    Color PLAYER_2_COLOR = new Color(0.8f, 0.8f, 1f);

    static public List<Collider2D> ball_check_colliders = new List<Collider2D>();

    public int my_number = 1;
    public float ball_offset_scale;
    public bool has_ball = false;
    public HUD.Team team;

    float max_speed = 5;
    public float acceleration = 30;
    public float dash_delay_time = 0.3f;
    public float dash_delay = 0;
    float current_shot_multiplier = 0;
    public float shot_force = 1000f;
    public float lose_control_force = 5f;
    public float charge_shot_delay = 1.5f;
    public float charge_shot_multiplier = 2f;
    public int charged_emit = 10;
    public float charged_speed = 10f;
    float dribble_speed = 0.8f;
    public float dribble_bad_speed = 0.01f;
    Vector2 current_ball_angle;
    Vector2 current_move_vector;
    GameObject teammate;

    float fatigue = 0;
    bool dash = false;
    bool shooting = false;
    bool dash_button_released = true;
    float shot_start_time;
    public bool isDashingCurrently = false;
    public int doge_luck = 80;
    public float doge_shot_force_mult = 10f;
    public float stolen_collision_delay = 0.75f;
    public float lose_ball_collision_delay = 0.1f;

    GameObject ball;
    Rigidbody2D ball_rb;
    Rigidbody2D rigid;
    CircleCollider2D player_collider;
    CircleCollider2D ball_collider;
    Color default_color;
    public GameObject label;

    public float pushSpeed = 3000f;
    public bool pushPoweruped = false;

    public bool isBaboon = false;
    public bool isDoge = false;

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player p = collision.gameObject.GetComponent<Player>();
            if (has_ball) {
                if (collision.relativeVelocity.magnitude > 5) {
                    loseControlOfBall();
                    Vector2 shot = ball.transform.position - transform.position;
                    Vector3.Normalize(shot);
                    shot *= lose_control_force;
                    ball.GetComponent<Rigidbody2D>().AddForce(shot);
                }
            }
            if ((p.team == HUD.Team.BLUE && team == HUD.Team.BLUE) || (p.team == HUD.Team.RED && team == HUD.Team.RED)) {
                Physics2D.IgnoreCollision(player_collider, collision.gameObject.GetComponent<Collider2D>());
                return;
            }

            if (!(pushPoweruped || isBaboon)) {
                return;
            }

            if ((p.team == HUD.Team.BLUE && team == HUD.Team.RED) || (p.team == HUD.Team.RED && team == HUD.Team.BLUE)) {
                float pushsp = pushSpeed;
                if (isDashingCurrently) {
                    pushsp *= 4f;
                }
                p.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * pushsp, ForceMode2D.Impulse);
            }
        }
        if (collision.gameObject.tag == "AsteroidBreakable" && ball.GetComponent<SoccerBall>().ball_in_play) {
            HUD.S.PlaySound("objecthit2", Random.Range(.5f, 1f));
            if (rigid.velocity.magnitude > 7.5f) {
                collision.gameObject.GetComponent<Asteriod>().Destroy();
            }
        }
    }

    void Awake() {
        if (my_number == 0 || my_number == 1) {
            team = HUD.Team.BLUE;
            label.GetComponent<SpriteRenderer>().color = PLAYER_2_COLOR;
        } else {
            team = HUD.Team.RED;
            label.GetComponent<SpriteRenderer>().color = PLAYER_1_COLOR;
        }
    }

    // should be the first thind done in anything that needs to be in a replay
    void Start() {
        ball = SoccerBall.Ball;
        ball_rb = ball.GetComponent<Rigidbody2D>();
        player_collider = GetComponent<CircleCollider2D>();
        ball_collider = ball.GetComponent<CircleCollider2D>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        current_shot_multiplier = charge_shot_multiplier;

        setTeammate();
        default_color = GetComponent<Renderer>().material.color;
        label.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("p" + (my_number + 1));
    }

    void setTeammate() {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            if (player.GetComponent<Player>().team == team && player != gameObject) {
                teammate = player;
                return;
            }
        }
    }

    void Update() {
        if (!HUD.S.GameStarted) {
            return;
        }
        checkDash();
        current_move_vector = ControlManager.getMovementVector(my_number);

        if (!ControlManager.fireButtonPressed(my_number)) {
            dash_button_released = true;
        }

        if (has_ball == true) {
            dribble();
        }
        if (ControlManager.fireButtonPressed(my_number) && has_ball && !shooting && dash_button_released) {
            startShot();
        }
        if (shooting && has_ball) {
            finishShot();
        }
        if (ControlManager.passButtonPressed(my_number) && has_ball) {
            passToTeammate();
        }
        if (team == HUD.Team.RED)
        {
            label.GetComponent<SpriteRenderer>().color = PLAYER_1_COLOR * (dash_delay + .75f);
        }
        else
        {
            label.GetComponent<SpriteRenderer>().color = PLAYER_2_COLOR * (dash_delay + .75f);
        }
    }

    void FixedUpdate() {
        updateMovement();
    }

    void passToTeammate() {
        Vector2 teammate_direction = teammate.transform.position - transform.position;
        current_ball_angle = teammate_direction.normalized;
        ball.transform.localPosition = current_ball_angle * ball_offset_scale;
        shot_start_time = int.MaxValue;
        actuallyShoot();
    }

    IEnumerator dashing() {
        isDashingCurrently = true;
        yield return new WaitForSeconds(0.8f);
        isDashingCurrently = false;
    }

    void updateMovement() {
        Vector2 move_vector = current_move_vector * acceleration;

        if (dash == true) {
            HUD.S.PlaySound("woosh", Random.Range(.5f, 1f));
            move_vector *= 20;
            StartCoroutine(dashing());
            dash = false;
        }/*
        if (slowed >0) {
            rigid.velocity = rigid.velocity/(1+slowed*0.2f);
        }
          */
        if (ball.GetComponent<SoccerBall>().ball_in_play) {
            rigid.AddForce(move_vector);
        }
        if (rigid.velocity.magnitude > max_speed && dash_delay < dash_delay_time * 0.8f) {
            rigid.velocity *= 0.97f;
        }
    }

    public void gainControlOfBall() {
        transform.GetChild(1).gameObject.SetActive(true);
        HUD.S.PlaySound("dribble", Random.Range(.5f, 1f));
        ball.transform.SetParent(transform);
        has_ball = true;
        current_ball_angle = (ball.transform.position - transform.position).normalized;
        dribble();
        ball_rb.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(player_collider, ball_collider, true);
        Physics2D.IgnoreCollision(ball.GetComponent<Collider2D>(), teammate.GetComponent<Collider2D>(), true);
    }

    public void loseControlOfBall(bool stolen = false) {
        transform.GetChild(1).gameObject.SetActive(false);
        if (ball.transform.parent != transform) {
            return;
        }

        HUD.S.stopLaserCharge();
        ControlManager.rumble(my_number, true);
        ball.transform.parent = null;
        has_ball = false;
        shooting = false;
        if (HUD.S.GameStarted && stolen) {
            Invoke("allowBallCollision", stolen_collision_delay);
        } else if (HUD.S.GameStarted) {
            Invoke("allowBallCollision", lose_ball_collision_delay);
        } else {
            Physics2D.IgnoreCollision(player_collider, ball_collider, false);
        }
        Physics2D.IgnoreCollision(ball.GetComponent<Collider2D>(), teammate.GetComponent<Collider2D>(), false);
    }

    void checkDash() {
        if (ControlManager.fireButtonPressed(my_number) && !has_ball && dash_delay <= 0 && dash_button_released) {
            dash = true;
            dash_button_released = false;
            dash_delay = dash_delay_time;
        } else if (dash_delay > 0) {
            dash_delay -= Time.deltaTime;
        }
    }

    void dribble() {
        Vector2 dribble_vector = ControlManager.getDribbleVector(my_number);
        if (dribble_vector.magnitude > DRIBBLE_MAGNITUDE_THRESHOLD) {
            current_ball_angle = dribble_vector.normalized;
        }

        if (current_ball_angle != Vector2.zero) {
            Vector3 ball_next_pos = current_ball_angle * ball_offset_scale;
            Vector3 diff = ball_next_pos - ball.transform.localPosition;
            float d_speed = dribble_speed;
            if (ball.GetComponent<SoccerBall>().hit_wall) {
                d_speed = dribble_bad_speed;
            }

            while (diff.magnitude > d_speed && diff.magnitude < 0.63f) {
                diff *= 0.99f;
            }
            ball_next_pos = ball.transform.localPosition + diff;
            //    ball.transform.localPosition =  Vector3.Lerp(ball.transform.localPosition,current_ball_angle * ball_offset_scale, Time.deltaTime * 20f);
            ball.transform.localPosition = Vector3.Lerp(ball.transform.localPosition, ball_next_pos, Time.deltaTime * 20f);

        }
    }

    void fitBallInStage(Vector2 old_position) {
        List<Collider2D> colliding_objects = new List<Collider2D>();
        foreach (Collider2D coll in ball_check_colliders) {
            if (coll.bounds.Intersects(ball_collider.bounds)) {
                colliding_objects.Add(coll);
            }
        }

        if (colliding_objects.Count == 0) {
            return;
        }

        float total_degree_movement = 0f;
        float current_angle = Util.getAngleInRads(ball.transform.localPosition);
        float fitting_interval = 0.05f * Util.getCloserDirection(current_angle, Util.getAngleInRads(old_position));
        float end_angle = Util.getAngleInRads(old_position);
        while (Util.colliderIntersects(ball_collider, colliding_objects) && total_degree_movement < (2 * Mathf.PI)) {
            current_angle += fitting_interval;
            total_degree_movement += fitting_interval;
            ball.transform.localPosition = new Vector2(Mathf.Cos(current_angle), Mathf.Sin(current_angle)) * ball_offset_scale;
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
            current_shot_multiplier = Mathf.Lerp(1f, charge_shot_multiplier, charge_time / charge_shot_delay);

            if (charge_time > charge_shot_delay) {
                ControlManager.rumble(my_number);
                ball.GetComponent<ParticleSystem>().Emit(charged_emit / 2);
                ball.GetComponent<ParticleSystem>().startSpeed = charged_speed;
            } else {
                ball.GetComponent<ParticleSystem>().Emit((int)(charge_time / charge_shot_delay * charged_emit / 2));
                ball.GetComponent<ParticleSystem>().startSpeed = (int)(charged_speed * charge_time / charge_shot_delay);
            }
        }
    }

    void actuallyShoot() {
        HUD.S.PlaySound("kick", Random.Range(.5f, 1f));
        loseControlOfBall();
        Vector2 shot = ball.transform.position - transform.position;
        Vector3.Normalize(shot);
        shot *= getShotForce() * current_shot_multiplier;
        if (Time.time - shot_start_time > charge_shot_delay) {
            ball.GetComponent<SoccerBall>().fadeParticles(charged_emit);
            HUD.S.fireLaser();
        }
        ball_rb.AddForce(shot);
        has_ball = false;
        shooting = false;
        current_shot_multiplier = 1f;
    }

    float getShotForce() {
        if (isDoge && Random.Range(0, 100) < doge_luck) {
            HUD.S.PlaySound("lucky", 1f);
            CameraShaker.S.DoShake(0.1f, 0.15f);
            return shot_force * doge_shot_force_mult;
        }
        return shot_force;
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

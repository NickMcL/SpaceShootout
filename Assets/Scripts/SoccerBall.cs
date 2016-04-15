using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoccerBall : MonoBehaviour {
    public static GameObject Ball;

    Rigidbody2D rb;
    Rigidbody ballrb;
    float max_speed = 50f;
    Vector3 WayToGo;

    public GameObject bgm_game_object;
    public int lastPlayerTouched;

    public Rigidbody2D parentrb;
    Collider2D col;

    public HUD.Team ball_team = HUD.Team.NONE;

    public float emit_decay_rate;
    public float time_scale_exponent = 2f;
    public float time_scale_max_dist = 5f;
    public float time_scale_min = 0.5f;
    public bool ball_in_play;
    //public float slow_mo_dist = 5f;

    bool fade_particles = false;
    public bool hit_wall = false;
    float hit_wall_cooldown = 0;
    float hit_wall_delay = 0.03f;
    float emit;
    GameObject[] goals;
    ParticleSystem particle_system;
    AudioSource bgm;
    public float max_homing_force = 30f; //if you set this too high things get silly

    public GameObject passing_target = null;

    // Use this for initialization
    void Awake() {
        Ball = gameObject;
    }

    void Start() {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        particle_system = GetComponent<ParticleSystem>();
        ballrb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
        bgm = bgm_game_object.GetComponent<AudioSource>();
        goals = GameObject.FindGameObjectsWithTag("Goal");
        ball_in_play = false;
        StartCoroutine(RayCastEveryFrame());
    }
    public LayerMask goalLayer;
    IEnumerator RayCastEveryFrame() {
        while (gameObject.active) {
            Vector3 prevloc = transform.position;

            yield return new WaitForFixedUpdate();

            RaycastHit2D[] hits = Physics2D.RaycastAll(prevloc, transform.position - prevloc, (transform.position - prevloc).magnitude, goalLayer);
            for (int c = 0; c < hits.Length; ++c) {
                if (hits[c].collider.gameObject.CompareTag("Goal")) {
                    hits[c].collider.GetComponent<Goal>().OnTriggerEnter2D(col);

                }
            }
        }
    }

    void FixedUpdate() {
        if (rb.velocity.magnitude > max_speed) {
            rb.velocity *= 0.99f;
        }

        if (transform.parent != null) {
            WayToGo.x = parentrb.velocity.y;
            WayToGo.y = -parentrb.velocity.x;
        } else {
            WayToGo.x = rb.velocity.y;
            WayToGo.y = -rb.velocity.x;
        }
        ballrb.angularVelocity = WayToGo;

        if (fade_particles) {
            particle_system.Emit((int)emit);
            emit /= emit_decay_rate;
            if (emit == 0) {
                fade_particles = false;
            }
        }

        if (hit_wall) {

            if (hit_wall_cooldown <= 0)
                hit_wall = false;
            hit_wall_cooldown -= Time.deltaTime;
        }

        if (passing_target != null) {
            // Put ball homing to teammate code here
            Vector3 to_target = passing_target.transform.position - transform.position;
            Vector3 target_vel = passing_target.GetComponent<Rigidbody2D>().velocity;
            Vector3 homing_target = to_target + target_vel;
            Vector3 my_vel = rb.velocity;

            //force should be applied perpendicular to velocity and should increase in power when closer to the target
            my_vel = Quaternion.Euler(0, 0, -90) * my_vel;
            Vector2 homing_force = Vector3.Project(homing_target, my_vel);
            homing_force = homing_force * homing_force.magnitude * homing_force.magnitude / (1f + to_target.magnitude * to_target.magnitude / 6);

            //prevent stupid dash bugs
            if (homing_force.magnitude > max_homing_force) {
                homing_force.Normalize();
                homing_force *= max_homing_force;
            }
            //apply force
            rb.AddForce(homing_force);
            Vector3 debug_vec = homing_force;
            Debug.DrawRay(transform.position, debug_vec, Color.yellow, 0.2f);

            //ball stops homing if it overshoots the player
            Vector3 diff = rb.velocity * 0.05f;
            diff -= to_target;
            if (to_target.magnitude < diff.magnitude) {
                passing_target = null;
            }
            //complaints regarding formatting are invalid as c# isnt my native language
        }
    }

    public void setPassingTarget(GameObject target) {
        passing_target = target;
    }

    // Update is called once per frame
    void Update() {
        timeDilation();
        if (transform.parent != null) {
			GetComponent<TrailRenderer> ().enabled = false;
			transform.GetChild(0).GetComponent<TrailRenderer> ().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
            Statistics.S.timeControlStat(transform.parent.GetComponent<Player>().my_number);
            passing_target = null;
        } else {
            transform.GetChild(1).gameObject.SetActive(true);
			GetComponent<TrailRenderer> ().enabled = true;
			transform.GetChild(0).GetComponent<TrailRenderer> ().enabled = true;
        }

    }

    public void fadeParticles(float start_emit) {
        fade_particles = true;
        emit = start_emit;
    }

    void timeDilation() {
        if (!ball_in_play || !HUD.S.GameStarted) {
            return;
        } else if (transform.parent != null) {
            Time.timeScale = bgm.pitch = 1f;
            return;
        }

        float goal_dist = getDistanceFromGoal();
        float slope = (1f - time_scale_min) / (Mathf.Pow(time_scale_max_dist, time_scale_exponent));
        float new_time_scale = slope * Mathf.Pow(goal_dist, time_scale_exponent) + time_scale_min;
        Time.timeScale = Mathf.Max(time_scale_min, Mathf.Min(1f, new_time_scale));
        bgm.pitch = Time.timeScale + ((1f - Time.timeScale) * 0.5f);

        /*Time.timeScale = 1;
        float velocity = rb.velocity.magnitude;
        if (transform.parent != null) {
            velocity = parentrb.velocity.magnitude;
        }

        foreach (GameObject goal in goals) {
            Vector3 dist = goal.transform.position - transform.position;
            dist.z = 0;
            float multiplier = (int)(dist.magnitude * 4 / slow_mo_dist) * 0.2f + 0.25f;
            if (dist.magnitude < slow_mo_dist) {
                multiplier *= (1 - velocity / max_speed);
                if (multiplier < 0.1f)
                    multiplier = 0.1f;
                if (Time.timeScale > multiplier) {
                    Time.timeScale = multiplier;
                }
            }
        }*/
    }

    float getDistanceFromGoal() {
        Vector2 temp;
        Vector2 min_distance_vector = Vector2.one * int.MaxValue;

        foreach (GameObject goal in goals) {
            temp = (Vector2)transform.position - (Vector2)goal.transform.position;
            if (temp.magnitude < min_distance_vector.magnitude) {
                min_distance_vector = temp;
            }
        }
        return min_distance_vector.magnitude;
    }
    public void beingStolen() {
        hit_wall = true;
        hit_wall_cooldown = hit_wall_delay * 2f;
    }
    void OnCollisionEnter2D(Collision2D coll) {
        bool stolen = false;
        passing_target = null;
        if (coll.gameObject.tag == "Player" && HUD.S.GameStarted) {
            Player coll_player = coll.gameObject.GetComponent<Player>();
            if (transform.parent != null && coll_player.team == ball_team) {
                return;
            }
            if (transform.parent != null) {
                if (ball_team != HUD.Team.NONE && coll_player.team != ball_team) {
                    HUD.S.SuccessfulSteal();
                    Statistics.S.stealStat(coll_player.my_number);
                    stolen = true;
                }
                transform.parent.gameObject.GetComponent<Player>().loseControlOfBall(stolen);
            }

            coll.gameObject.GetComponent<Player>().gainControlOfBall();
            lastPlayerTouched = coll.gameObject.GetComponent<Player>().my_number;
            fade_particles = false;
            ball_team = coll_player.team;
            parentrb = coll.gameObject.GetComponent<Rigidbody2D>();
        }

        if (coll.gameObject.tag == "LevelBounds") {
            HUD.S.PlaySound("boing", Random.Range(.5f, 1f));
            hit_wall_cooldown = hit_wall_delay;
            hit_wall = true;
        }

        if (coll.gameObject.tag == "AsteroidBreakable" && ball_in_play) {
            HUD.S.PlaySound("objecthit2", Random.Range(.5f, 1f));
            if (rb.velocity.magnitude > 10f && transform.parent == null) {
                coll.gameObject.GetComponent<Asteriod>().Destroy();
            }
            hit_wall_cooldown = hit_wall_delay;
            hit_wall = true;
        }
        hit_wall_cooldown = hit_wall_delay;
        hit_wall = true;
    }
    void OnCollisionStay2D() {
        hit_wall_cooldown = hit_wall_delay;
        hit_wall = true;
    }
}
using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour {
    Rigidbody2D rb;
    Rigidbody ballrb;
    float max_speed = 50f;
    public static GameObject Ball;
    Vector3 WayToGo;

    public Rigidbody2D parentrb;

    public HUD.Team ball_team = HUD.Team.NONE;

    public float emit_decay_rate;
    public float time_scale_exponent = 2f;
    public float time_scale_max_dist = 5f;
    public float time_scale_min = 0.5f;
    public bool ball_in_play;
    //public float slow_mo_dist = 5f;

    bool fade_particles = false;
    float emit;
    GameObject[] goals;
    ParticleSystem particle_system;

    // Use this for initialization
    void Awake() {
        Ball = gameObject;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        particle_system = GetComponent<ParticleSystem>();
        ballrb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
        goals = GameObject.FindGameObjectsWithTag("Goal");
        ball_in_play = true;
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
    }

    // Update is called once per frame
    void Update() {
        timeDilation();
    }

    public void fadeParticles(float start_emit) {
        fade_particles = true;
        emit = start_emit;
    }

    void timeDilation() {
        if (!ball_in_play || !HUD.S.GameStarted) {
            return;
        } else if (transform.parent != null) {
            Time.timeScale = 1f;
            return;
        }

        float goal_dist = getDistanceFromGoal();
        float slope = (1f - time_scale_min) / (Mathf.Pow(time_scale_max_dist, time_scale_exponent));
        float new_time_scale = slope * Mathf.Pow(goal_dist, time_scale_exponent) + time_scale_min;
        Time.timeScale = Mathf.Max(time_scale_min, Mathf.Min(1f, new_time_scale));

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
            temp = (Vector2) transform.position - (Vector2) goal.transform.position;
            if (temp.magnitude < min_distance_vector.magnitude) {
                min_distance_vector = temp;
            }
        }
        return min_distance_vector.magnitude;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        bool stolen = false;

        if (coll.gameObject.tag == "Player" && HUD.S.GameStarted) {
            Player coll_player = coll.gameObject.GetComponent<Player>();
            if (transform.parent != null && coll_player.team == ball_team) {
                return;
            }
            if (transform.parent != null) {
                if (ball_team != HUD.Team.NONE && coll_player.team != ball_team) {
                    HUD.S.SuccessfulSteal();
                    stolen = true;
                }
                transform.parent.gameObject.GetComponent<Player>().loseControlOfBall(stolen);
            }

            coll.gameObject.GetComponent<Player>().gainControlOfBall();
            fade_particles = false;
            ball_team = coll_player.team;
            parentrb = coll.gameObject.GetComponent<Rigidbody2D>();
        }

        if (coll.gameObject.tag == "LevelBounds") {
            HUD.S.PlaySound("boing", Random.Range(.5f, 1f));
        }

        if (coll.gameObject.tag == "AsteroidBreakable") {
            HUD.S.PlaySound("objecthit2", Random.Range(.5f, 1f));
            if (rb.velocity.magnitude > 10f && transform.parent == null) {
                coll.gameObject.GetComponent<Asteriod>().Destroy();
            }
        }
    }
}
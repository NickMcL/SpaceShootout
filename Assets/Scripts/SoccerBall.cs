using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour {
    Rigidbody2D rb;
    Rigidbody ballrb;
    float max_speed = 50;
    public static GameObject Ball;
    Vector3 WayToGo;

    

    public bool BlueBall = false;

    // Use this for initialization
    void Awake() {
        Ball = gameObject;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ballrb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (transform.parent != null)
        {
            WayToGo.x = parentrb.velocity.y;
            WayToGo.y = -parentrb.velocity.x;
        }
        else {
            WayToGo.x = rb.velocity.y;
            WayToGo.y = -rb.velocity.x;
        }
        ballrb.angularVelocity = WayToGo;
    }

    // Update is called once per frame
    void Update() {
        while (rb.velocity.magnitude > max_speed) {
            rb.velocity *= 0.99f;
        }
        GetComponent<ParticleSystem>().Emit((int)( rb.velocity.magnitude/2));
    }
    Rigidbody2D parentrb;
    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player" && HUD.S.GameStarted) {
            if ((coll.gameObject.GetComponent<Player>().RedTeam && BlueBall) || (!coll.gameObject.GetComponent<Player>().RedTeam && !BlueBall))
            {
                HUD.S.SuccessfulBlock();
            }
            if (transform.parent != null) {
            
                transform.parent.gameObject.GetComponent<Player>().loseControlOfBall();
            }
            coll.gameObject.GetComponent<Player>().gainControlOfBall();
            if (coll.gameObject.GetComponent<Player>().RedTeam)
            {
                BlueBall = false;
            } else
            {
                BlueBall = true;
            }
            parentrb = coll.gameObject.GetComponent<Rigidbody2D>();
        }
        if(coll.gameObject.tag == "LevelBounds")
        {
            HUD.S.PlaySound("boing", Random.Range(.5f,1f));
        }
        if (coll.gameObject.tag == "Asteroid")
        {
            HUD.S.PlaySound("objecthit2", Random.Range(.5f,1f));
            if (rb.velocity.magnitude > 10f)
            {
                coll.gameObject.GetComponent<Asteriod>().Destroy();
            }
        }
    }
}

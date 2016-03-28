using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
    public Text middleText;
    public Text p1scoretext, p2scoretext;
    public Color goalColorRed, goalColorBlue;
    public Player player1red;
    public Player player1blue;

    public Text countdown;

    public List<Goal> goals = new List<Goal>();

    public GameObject ball;
    public bool player2isGoalie = true;

    public static HUD S;

    public float round_time = 10f;
    public float TimeLeft;
    public float end_round_delay = 3f;

    public bool secondhalf = false;

    void Awake() {
        S = this;
    }

    // Use this for initialization
    void Start() {
        TimeLeft = round_time;
        PlaySound("LaserMillenium", .25f);
        StartCoroutine(Count_Down());
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        for (int c = 0; c < g.Length; ++c) {
            Player p = g[c].GetComponent<Player>();
            if (p.my_number == 1) {
                player1red = p;
            } else {
                player1blue = p;
            }
        }
        ball = GameObject.FindGameObjectWithTag("Ball");

        GameObject[] gs = GameObject.FindGameObjectsWithTag("Goal");
        goals.Add(gs[0].GetComponent<Goal>());

        goals.Add(gs[1].GetComponent<Goal>());
    }

    public int BlueTeamScore = 0;
    public int RedTeamScore = 0;

    public void UpdateScores() {
        if (secondhalf)
        {

            p2scoretext.text = BlueTeamScore.ToString();
            p1scoretext.text = RedTeamScore.ToString();
        }
        else {
            p1scoretext.text = BlueTeamScore.ToString();
            p2scoretext.text = RedTeamScore.ToString();
        }
    }


    public void EveryoneLoseControl()
    {
        player1blue.GetComponent<Player>().loseControlOfBall();
        player1red.GetComponent<Player>().loseControlOfBall();
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    public void RedTeamScored() {
        GameStarted = false;
        ++RedTeamScore;
        middleText.text = "Red Team Scores!";
        EveryoneLoseControl();
        StartCoroutine(erasetextin(1f));
        UpdateScores();
        StartCoroutine(GameReset(false));
    }

    public void BlueTeamScored() {
        GameStarted = false;
        ++BlueTeamScore;
         
        middleText.text = "Blue Team Scores!";
        EveryoneLoseControl();
        StartCoroutine(erasetextin(1f));
        UpdateScores();
        StartCoroutine(GameReset(true));
    }

    void erasetext() {
        middleText.text = "";
    }
    
    IEnumerator erasetextin(float f)
    {
        yield return new WaitForSeconds(f);
        middleText.text = "";
    }

    public bool GameStarted = false;
    IEnumerator Count_Down() {
        middleText.text = "3";
        PlaySound("close02", 1f);
        yield return new WaitForSeconds(1f);
        middleText.text = "2";
        PlaySound("close02", 1f);
        yield return new WaitForSeconds(1f);
        middleText.text = "1";
        PlaySound("close02", 1f);
        yield return new WaitForSeconds(1f);
        CameraShaker.S.DoShake(0.05f, 0.15f);
        EveryoneLoseControl();
        middleText.text = "Go!";
        PlaySound("select01", 1f);
        yield return new WaitForSeconds(0.4f);
        middleText.text = "";
        GameStarted = true;
    }

    IEnumerator GameReset(bool BlueScored)
    {
        GameStarted = false;
        yield return new WaitForSeconds(1f);
        if (secondhalf)
        {
            player1red.transform.position = BlueTeamStartPos1;
            player1blue.transform.position = RedTeamStartPos1;
            if (BlueScored)
            {
                ball.transform.position = BallStartPosBlueAdvantage;
            }
            else
            {
                ball.transform.position = BallStartPosRedAdvantage;

            }
        }
        else {
            player1red.transform.position = RedTeamStartPos1;
            player1blue.transform.position = BlueTeamStartPos1;
            if (BlueScored)
            {
                ball.transform.position = BallStartPosRedAdvantage;
            }
            else
            {
                ball.transform.position = BallStartPosBlueAdvantage;
            }
        }
        StartCoroutine(Count_Down());
    }

    IEnumerator Halftime()
    {
        GameStarted = false;
        middleText.text = "Half Time!";
        secondhalf = true;
        yield return new WaitForSeconds(1f);

        player1red.transform.position = BlueTeamStartPos1;
        player1blue.transform.position = RedTeamStartPos1;

        ball.transform.position = Vector2.zero;

        if (goals[0].redGoal)
        {
            goals[0].redGoal = false;
            goals[0].GetComponent<SpriteRenderer>().color = goalColorBlue;
            goals[1].redGoal = true;

            goals[1].GetComponent<SpriteRenderer>().color = goalColorRed;
        } else
        {
            goals[0].redGoal = true;

            goals[0].GetComponent<SpriteRenderer>().color = goalColorRed;
            goals[1].redGoal = false;
            goals[1].GetComponent<SpriteRenderer>().color = goalColorBlue;
        }

        StartCoroutine(Count_Down());
    }

    public void PlaySound(string name, float volume = 1f) {
        GameObject g = new GameObject();
        AudioSource adsrc = g.AddComponent<AudioSource>();
        g.transform.position = Camera.main.transform.position;
        adsrc.spatialBlend = 0;
        AudioClip ac = Resources.Load("Sound/" + name) as AudioClip;
        adsrc.clip = ac;
        adsrc.volume = volume;
        adsrc.Play();
        Destroy(g, ac.length);
    }

    // Update is called once per frame
    void Update() {

    }

    public Vector3 RedTeamStartPos1, RedTeamStartPos2;
    public Vector3 BlueTeamStartPos1, BlueTeamStartPos2;
    public Vector3 BallStartPosBlueAdvantage, BallStartPosRedAdvantage;

    public void SuccessfulBlock() {
        middleText.text = "Nice!";
        CameraShaker.S.DoShake(0.04f, 0.15f);
        StartCoroutine(erasetextin(0.2f));
    }

    IEnumerator GameEnded()
    {
        GameStarted = false;
        middleText.text = "Time's Up!";
        CameraShaker.S.DoShake(0.09f, 0.15f);
        yield return new WaitForSeconds(1f);
        if(RedTeamScore > BlueTeamScore)
        {
            middleText.text = "Red Team Wins!";
            
            yield return new WaitForSeconds(2f);
            

        } else if (BlueTeamScore < RedTeamScore)
        {
            middleText.text = "Blue Team Wins!";

            yield return new WaitForSeconds(2f);
        } else
        {

            middleText.text = "Tie!";

            yield return new WaitForSeconds(2f);
        }

        Application.LoadLevel(0);
    }

    void FixedUpdate() {
        if (!GameStarted) {
            return;
        }

        countdown.text = TimeLeft.ToString("F2");
        if (TimeLeft > 0f) {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0f) {
                if (secondhalf)
                {
                    StartCoroutine(GameEnded());
                    TimeLeft = float.MaxValue;
                    countdown.text = "";
                }
                else
                {
                    TimeLeft = round_time;
                    StartCoroutine(Halftime());
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {
    List<string> BALL_STOLEN_STRINGS = new List<string>() {
        "Stolen!", "Turnover!", "Robbed!", "Hijacked!", "Swiped!", "Snatched!", "Bamboozled!"
    };

    public Text middleText;
    public Text p1scoretext, p2scoretext;
    public Color goalColorRed, goalColorBlue;
    public Player player1red, player2red;
    public Player player1blue, player2blue;

    public Vector3 RedTeamStartPos1, RedTeamStartPos2;
    public Vector3 BlueTeamStartPos1, BlueTeamStartPos2;
    public Vector3 BallStartPosBlueAdvantage, BallStartPosRedAdvantage;

    public Text countdown;

    public List<Goal> goals = new List<Goal>();

    public GameObject ball;
    public bool player2isGoalie = true;

    public static HUD S;

    public float round_time = 10f;
    public float TimeLeft;
    public float end_round_delay = 3f;
    public bool GameStarted = false;
    public int BlueTeamScore = 0;
    public int RedTeamScore = 0;
    public float score_time_scale = 0.5f;

    public bool secondhalf = false;

    public enum Team { BLUE, RED, NONE };

    GameObject laser_sound;

    Dictionary<int, Player> playersAndNums = new Dictionary<int, Player>();
    List<Collider2D> asteroidboxes = new List<Collider2D>();
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
            if (p.my_number == 0) {
                player1blue = p;
                playersAndNums[0] = p;
            } else if (p.my_number == 1) {
                player2blue = p;
                playersAndNums[1] = p;
            } else if (p.my_number == 2) {
                player1red = p;
                playersAndNums[2] = p;
            } else {
                player2red = p;
                playersAndNums[3] = p;
            }
        }
        Physics2D.IgnoreCollision(playersAndNums[0].GetComponent<Collider2D>(), playersAndNums[1].GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(playersAndNums[2].GetComponent<Collider2D>(), playersAndNums[3].GetComponent<Collider2D>());
        player1red.transform.position = RedTeamStartPos1;
        player2red.transform.position = RedTeamStartPos2;
        player1blue.transform.position = BlueTeamStartPos1;
        player2blue.transform.position = BlueTeamStartPos2;
        ball = GameObject.FindGameObjectWithTag("Ball");

        GameObject[] gs = GameObject.FindGameObjectsWithTag("Goal");
        goals.Add(gs[0].GetComponent<Goal>());

        goals.Add(gs[1].GetComponent<Goal>());

        GameObject[] asts = GameObject.FindGameObjectsWithTag("Asteroid");
        for(int c = 0; c < asts.Length; ++c)
        {
            asteroidboxes.Add(asts[c].GetComponent<Collider2D>());
        }
        GameObject[] asts2 = GameObject.FindGameObjectsWithTag("AsteroidBreakable");
        for(int c = 0; c < asts2.Length; ++c)
        {
            asteroidboxes.Add(asts2[c].GetComponent<Collider2D>());
        }
        StartCoroutine(SpawnPowerups());

        Global.S.loadSprites();
        

    }

    public GameObject[] PowerUps;
    bool powerUpOut = false;
    float StageLengthX = 14f;
    float StageLengthY = 8f;

    public bool PointIsNearPlayers(Vector3 point, float maxdist)
    {
        foreach (KeyValuePair<int, Player> entry in playersAndNums)
        {
            if((entry.Value.transform.position - point).magnitude < maxdist){
                return true;
            }
        }

        foreach(Collider2D col in asteroidboxes)
        {
            if (col != null)
            {
                if (col.bounds.Contains(point))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public float SpawnPowerupIntervalMax = 15f;
    public float SpawnPowerupIntervalMin = 5f;

    public IEnumerator SpawnPowerups()
    {
        while (true)
        {
            if (!powerUpOut)
            {
                yield return new WaitForSeconds(Random.Range(SpawnPowerupIntervalMin, SpawnPowerupIntervalMax));
                int rnjesus = Random.Range(0, PowerUps.Length);
                Vector3 targetPos = new Vector3(Random.Range(-StageLengthX / 2f, StageLengthX / 2f), Random.Range(-StageLengthY / 2f, StageLengthY / 2f));
                bool TooCloseToPlayers = PointIsNearPlayers(targetPos, 2f);



                while (TooCloseToPlayers)
                {
                    targetPos = new Vector3(Random.Range(-StageLengthX / 2f, StageLengthX / 2f), Random.Range(-StageLengthY / 2f, StageLengthY / 2f));
                    TooCloseToPlayers = PointIsNearPlayers(targetPos, 2f);
                }

                Instantiate(PowerUps[rnjesus], targetPos, transform.rotation);

                powerUpOut = true;
            } else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void UpdateScores() {
        p1scoretext.text = BlueTeamScore.ToString();
        p2scoretext.text = RedTeamScore.ToString();
    }

    public void EveryoneLoseControl() {
        player1blue.GetComponent<Player>().loseControlOfBall();
        player1red.GetComponent<Player>().loseControlOfBall();
        player2red.GetComponent<Player>().loseControlOfBall();
        player2blue.GetComponent<Player>().loseControlOfBall();
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void RedTeamScored() {
        Time.timeScale = score_time_scale;
        GameStarted = false;
        ++RedTeamScore;
        middleText.text = "Red Team Scores!";
        EveryoneLoseControl();
        StartCoroutine(erasetextin(1f));
        UpdateScores();
        player1red.burstRumble(1f);
        player2red.burstRumble(1f);
        StartCoroutine(GameReset(Team.RED));
    }

    public void BlueTeamScored() {
        Time.timeScale = score_time_scale;
        GameStarted = false;
        ++BlueTeamScore;

        middleText.text = "Blue Team Scores!";
        EveryoneLoseControl();
        StartCoroutine(erasetextin(1f));
        UpdateScores();
        player1blue.burstRumble(1f);
        player2blue.burstRumble(1f);
        StartCoroutine(GameReset(Team.BLUE));
    }

    void erasetext() {
        middleText.text = "";
    }

    IEnumerator erasetextin(float f) {
        yield return new WaitForSeconds(f);
        middleText.text = "";
    }

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
        ball.GetComponent<SoccerBall>().ball_in_play = true;
    }

    IEnumerator GameReset(Team scoring_team) {
        Time.timeScale = 1f;
        GameStarted = false;
        yield return new WaitForSeconds(1f);
        ResetObjects.S.Reset();
        player1red.transform.position = RedTeamStartPos1;
        player2red.transform.position = RedTeamStartPos2;
        player1blue.transform.position = BlueTeamStartPos1;
        player2blue.transform.position = BlueTeamStartPos2;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (scoring_team == Team.RED) {
            ball.transform.position = BallStartPosRedAdvantage;
        } else {
            ball.transform.position = BallStartPosBlueAdvantage;
        }
        StartCoroutine(Count_Down());
    }

    /*IEnumerator Halftime() {
        GameStarted = false;
        middleText.text = "Half Time!";
        secondhalf = true;
        yield return new WaitForSeconds(1f);

        player1red.transform.position = BlueTeamStartPos1;
        player1blue.transform.position = RedTeamStartPos1;

        ball.transform.position = Vector2.zero;

        if (goals[0].team == Team.RED) {
            goals[0].team = Team.BLUE;
            goals[1].team = Team.RED;
            goals[0].GetComponent<SpriteRenderer>().color = goalColorBlue;

            goals[1].GetComponent<SpriteRenderer>().color = goalColorRed;
        } else {
            goals[0].team = Team.RED;
            goals[1].team = Team.BLUE;

            goals[0].GetComponent<SpriteRenderer>().color = goalColorRed;
            goals[1].GetComponent<SpriteRenderer>().color = goalColorBlue;
        }

        StartCoroutine(Count_Down());
    }*/

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

    public void startLaserCharge() {
        laser_sound = new GameObject();
        Invoke("playLaserCharge", 0.3f);
    }

    void playLaserCharge() {
        if (laser_sound == null) {
            return;
        }
        AudioSource adsrc = laser_sound.AddComponent<AudioSource>();
        laser_sound.transform.position = Camera.main.transform.position;
        adsrc.spatialBlend = 0;
        AudioClip ac = Resources.Load("Sound/charging_laser") as AudioClip;
        adsrc.clip = ac;
        adsrc.volume = 1f;
        adsrc.Play();
    }

    public void stopLaserCharge() {
        if (laser_sound == null) {
            return;
        }

        AudioSource audio_source = laser_sound.GetComponent<AudioSource>();
        laser_sound = null;
        if (audio_source == null) {
            return;
        }
        audio_source.Stop();
    }

    public void fireLaser() {
        stopLaserCharge();
        PlaySound("fire_laser", 1f);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ControlManager.use_controllers = !ControlManager.use_controllers;
        }
    }

    public void SuccessfulSteal() {
        middleText.text = BALL_STOLEN_STRINGS[Random.Range(0, BALL_STOLEN_STRINGS.Count)];
        CameraShaker.S.DoShake(0.04f, 0.15f);
        StartCoroutine(erasetextin(0.2f));
    }

    public void GetSpeedPowerup()
    {
        middleText.text = "SPEED BOOST!";
        CameraShaker.S.DoShake(0.04f, 0.15f);
        StartCoroutine(erasetextin(0.2f));
        powerUpOut = false;
    }

    public void GetPushPowerup()
    {
        middleText.text = "TACKLE BOOST!";
        CameraShaker.S.DoShake(0.04f, 0.15f);
        StartCoroutine(erasetextin(0.2f));
        powerUpOut = false;
    }

    public void GetShootPowerup()
    {
        middleText.text = "SHOOT BOOST!";
        CameraShaker.S.DoShake(0.04f, 0.15f);
        StartCoroutine(erasetextin(0.2f));
        powerUpOut = false;
    }

    IEnumerator GameEnded() {
        GameStarted = false;
        Time.timeScale = 1;
        ball.GetComponent<SoccerBall>().ball_in_play = false;
        middleText.text = "Time's Up!";
        CameraShaker.S.DoShake(0.09f, 0.15f);
        yield return new WaitForSeconds(1f);
        if (RedTeamScore > BlueTeamScore) {
            middleText.text = "Red Team Wins!";

            yield return new WaitForSeconds(2f);


        } else if (BlueTeamScore > RedTeamScore) {
            middleText.text = "Blue Team Wins!";

            yield return new WaitForSeconds(2f);
        } else {

            middleText.text = "Tie!";

            yield return new WaitForSeconds(2f);
        }

        SceneManager.LoadScene("CharacterSelect");
    }
    public Image overlay;
    IEnumerator FlashRed()
    {
        Color c = overlay.color;
        c.a = 1f;
        overlay.color = c;
        for(int cee = 0; cee < 5; ++cee)
        {
            yield return new WaitForSeconds(0.1f);
            c.a -= 0.2f;
            overlay.color = c;
        }
    }
    bool doingFiveSecondsLeft = false;
    IEnumerator fiveSecondsLeft()
    {
        countdown.color = Color.red;
        doingFiveSecondsLeft = true;
        for(int c = 0; c < 10; ++c)
        {
            StartCoroutine(FlashRed());
            yield return new WaitForSeconds(1f);
        }

    }

    void FixedUpdate() {
        if (!GameStarted) {
            return;
        }

        countdown.text = TimeLeft.ToString("F2");
        if (TimeLeft > 0f) {
            TimeLeft -= Time.deltaTime;
            if(TimeLeft <= 10f && !doingFiveSecondsLeft)
            {
                doingFiveSecondsLeft = true;
                StartCoroutine(fiveSecondsLeft());
            }
            if (TimeLeft <= 0f) {
                StartCoroutine(GameEnded());
                TimeLeft = float.MaxValue;
                countdown.text = "";
            }
        }
    }
}

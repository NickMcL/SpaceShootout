using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Text middleText;
    public Text p1scoretext, p2scoretext;

    public Player player1;
    public Player player2;

    public Text countdown;

    public GameObject ball;
    public bool player2isGoalie = true;

    public static HUD S;

    public float TimeLeft = 15;

    void Awake()
    {
        S = this;
    }

	// Use this for initialization
	void Start () {
        PlaySound("LaserMillenium", 1f);
        StartCoroutine(Count_Down());
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        for(int c = 0; c < g.Length; ++c)
        {
            Player p = g[c].GetComponent<Player>();
            if(p.my_number == 1)
            {
                player1 = p;
            } else
            {
                player2 = p;
            }
        }
        ball = GameObject.FindGameObjectWithTag("Ball");
	}

    public int P1Score = 0;
    public int P2Score = 0;

    public void UpdateScores()
    {
        p1scoretext.text = P1Score.ToString();
        p2scoretext.text = P2Score.ToString();
    }

    public void Player1Scored()
    {
        ++P1Score;
        middleText.text = "Player 1 Scores!";
        StartCoroutine(erasetext(1f));
        UpdateScores();

        PlayaSwap();
    }

    public void Player2Scored()
    {
        ++P2Score;
        middleText.text = "Player 2 Scores!";
        StartCoroutine(erasetext(1f));

        UpdateScores();

        PlayaSwap();
    }

    IEnumerator erasetext(float time)
    {
        yield return new WaitForSeconds(time);
        middleText.text = "";
    }
    public bool GameStarted = false;
    IEnumerator Count_Down()
    {
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
        middleText.text = "Go!";
        PlaySound("select01", 1f);
        yield return new WaitForSeconds(0.4f);
        middleText.text = "";
        GameStarted = true;
    }

    public void PlaySound(string name, float volume = 1f)
    {
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
    void Update () {
	
	}

    public Vector3 GoalieStartPos;
    public Vector3 ShooterStartPos;
    public Vector3 BallStartPos;

    public void PlayaSwap()
    {
        if (player2isGoalie)
        {
            player2isGoalie = false;
            player1.is_goalie = true;
            player2.is_goalie = false;
            player1.transform.position = GoalieStartPos;
            player2.transform.position = ShooterStartPos;
        } else
        {
            player2isGoalie = true;
            player1.is_goalie = false;
            player2.is_goalie = true;
            player1.transform.position = ShooterStartPos;
            player2.transform.position = GoalieStartPos;
        }
        ball.transform.position = BallStartPos;
    }

    IEnumerator block()
    {
        middleText.text = "Nice Block!";
        yield return new WaitForSeconds(1f);
        middleText.text = "";
        PlayaSwap();
    }

    public void SuccessfulBlock()
    {
        StartCoroutine(block());
    }

    void FixedUpdate()
    {
        if (!GameStarted)
        {
            return;
        }
        TimeLeft -= Time.deltaTime;
        countdown.text = TimeLeft.ToString("F2");
        if(TimeLeft <= 0)
        {
            middleText.text = "Out of Time!";
            StartCoroutine(erasetext(1f));
            TimeLeft = 15f;
            PlayaSwap();
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Text middleText;
    public Text p1scoretext, p2scoretext;

    public static HUD S;


    void Awake()
    {
        S = this;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(Count_Down());
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
    }

    public void Player2Scored()
    {
        ++P2Score;
        middleText.text = "Player 2 Scores!";
        StartCoroutine(erasetext(1f));

        UpdateScores();
    }

    IEnumerator erasetext(float time)
    {
        yield return new WaitForSeconds(time);
        middleText.text = "";
    }

    IEnumerator Count_Down()
    {
        middleText.text = "3";
        yield return new WaitForSeconds(1f);
        middleText.text = "2";
        yield return new WaitForSeconds(1f);
        middleText.text = "1";
        yield return new WaitForSeconds(1f);
        middleText.text = "Go!";
        yield return new WaitForSeconds(0.4f);
        middleText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

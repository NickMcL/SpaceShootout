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
        PlaySound("Spacearray", 1f);
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
}

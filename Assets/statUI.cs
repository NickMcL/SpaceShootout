using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class statUI : MonoBehaviour {

    public Text player1Goals;
    public Text player1Steals;
    public Text player1Time;

    public Text player2Goals;
    public Text player2Steals;
    public Text player2Time;

    public Text player3Goals;
    public Text player3Steals;
    public Text player3Time;

    public Text player4Goals;
    public Text player4Steals;
    public Text player4Time;


    public Sprite bearWin, bearLose, fishWin, fishLose, baboonWin, baboonLose, hawkWin, hawkLose, dogeWin, dogeLose, foxWin, foxLose, bearTie, fishTie, baboonTie, hawkTie, dogeTie, foxTie;
    // blue p1, blue p2, red p1, red p2
    // Use this for initialization
    void Start () {
        PlaySound("SteppinUp", 1f);
        Invoke("canGo", 5f);
        player1Goals = GameObject.Find("Player1Goals").GetComponent<Text>();
        player1Steals = GameObject.Find("Player1Steals").GetComponent<Text>();
        player1Time = GameObject.Find("Player1Time").GetComponent<Text>();

        player2Goals = GameObject.Find("Player2Goals").GetComponent<Text>();
        player2Steals = GameObject.Find("Player2Steals").GetComponent<Text>();
        player2Time = GameObject.Find("Player2Time").GetComponent<Text>();

        player3Goals = GameObject.Find("Player3Goals").GetComponent<Text>();
        player3Steals = GameObject.Find("Player3Steals").GetComponent<Text>();
        player3Time = GameObject.Find("Player3Time").GetComponent<Text>();

        player4Goals = GameObject.Find("Player4Goals").GetComponent<Text>();
        player4Steals = GameObject.Find("Player4Steals").GetComponent<Text>();
        player4Time = GameObject.Find("Player4Time").GetComponent<Text>();



        player1Goals.text += Statistics.goalsScored[0];
        player1Steals.text += Statistics.steals[0];
        player1Time.text += Mathf.Floor(Statistics.timeControlled[0]);
        player1Time.text += " sec.";

        player2Goals.text += Statistics.goalsScored[1];
        player2Steals.text += Statistics.steals[1];
        player2Time.text += Mathf.Floor(Statistics.timeControlled[1]);
        player2Time.text += " sec.";

        player3Goals.text += Statistics.goalsScored[2];
        player3Steals.text += Statistics.steals[2];
        player3Time.text += Mathf.Floor(Statistics.timeControlled[2]);
        player3Time.text += " sec.";

        player4Goals.text += Statistics.goalsScored[3];
        player4Steals.text += Statistics.steals[3];
        player4Time.text += Mathf.Floor(Statistics.timeControlled[3]);
        player4Time.text += " sec.";

        if (Global.S.TIE)
        {
            if (Global.S.BlueP1 == "Bear")
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = bearTie;
            }
            else if (Global.S.BlueP1 == "Fox")
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = foxTie;
            }
            else if (Global.S.BlueP1 == "Hawk")
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = hawkTie;
            }
            else if (Global.S.BlueP1 == "Baboon")
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = baboonTie;
            }
            else if (Global.S.BlueP1 == "Fish")
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = fishTie;
            }
            else
            {
                GameObject.Find("Player1Image").GetComponent<Image>().sprite = dogeTie;
            }

            if (Global.S.BlueP2 == "Bear")
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = bearTie;
            }
            else if (Global.S.BlueP2 == "Fox")
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = foxTie;
            }
            else if (Global.S.BlueP2 == "Hawk")
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = hawkTie;
            }
            else if (Global.S.BlueP2 == "Baboon")
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = baboonTie;
            }
            else if (Global.S.BlueP2 == "Fish")
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = fishTie;
            }
            else
            {
                GameObject.Find("Player2Image").GetComponent<Image>().sprite = dogeTie;
            }

            if (Global.S.RedP1 == "Bear")
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = bearTie;
            }
            else if (Global.S.RedP1 == "Fox")
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = foxTie;
            }
            else if (Global.S.RedP1 == "Hawk")
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = hawkTie;
            }
            else if (Global.S.RedP1 == "Baboon")
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = baboonTie;
            }
            else if (Global.S.RedP1 == "Fish")
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = fishTie;
            }
            else
            {
                GameObject.Find("Player3Image").GetComponent<Image>().sprite = dogeTie;
            }

            if (Global.S.RedP2 == "Bear")
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = bearTie;
            }
            else if (Global.S.RedP2 == "Fox")
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = foxTie;
            }
            else if (Global.S.RedP2 == "Hawk")
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = hawkTie;
            }
            else if (Global.S.RedP2 == "Baboon")
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = baboonTie;
            }
            else if (Global.S.RedP2 == "Fish")
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = fishTie;
            }
            else
            {
                GameObject.Find("Player4Image").GetComponent<Image>().sprite = dogeTie;
            }



            return;
        }


        if(Global.S.BlueP1 == "Bear")
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? bearLose : bearWin);
        } else if (Global.S.BlueP1 == "Fox")
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? foxLose : foxWin);
        }
        else if (Global.S.BlueP1 == "Hawk")
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? hawkLose : hawkWin);
        }
        else if (Global.S.BlueP1 == "Baboon")
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? baboonLose : baboonWin);
        }
        else if (Global.S.BlueP1 == "Fish")
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? fishLose: fishWin);
        }
        else
        {
            GameObject.Find("Player1Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? dogeLose : dogeWin);
        }


        if (Global.S.BlueP2 == "Bear")
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? bearLose : bearWin);
        }
        else if (Global.S.BlueP2 == "Fox")
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? foxLose : foxWin);
        }
        else if (Global.S.BlueP2 == "Hawk")
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? hawkLose : hawkWin);
        }
        else if (Global.S.BlueP2 == "Baboon")
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? baboonLose : baboonWin);
        }
        else if (Global.S.BlueP2 == "Fish")
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? fishLose : fishWin);
        }
        else
        {
            GameObject.Find("Player2Image").GetComponent<Image>().sprite = ((Global.S.REDISWINRAR) ? dogeLose : dogeWin);
        }


        if (Global.S.RedP1 == "Bear")
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? bearLose : bearWin);
        }
        else if (Global.S.RedP1 == "Fox")
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? foxLose : foxWin);
        }
        else if (Global.S.RedP1 == "Hawk")
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? hawkLose : hawkWin);
        }
        else if (Global.S.RedP1 == "Baboon")
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? baboonLose : baboonWin);
        }
        else if (Global.S.RedP1 == "Fish")
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? fishLose : fishWin);
        }
        else
        {
            GameObject.Find("Player3Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? dogeLose : dogeWin);
        }


        if (Global.S.RedP2 == "Bear")
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? bearLose : bearWin);
        }
        else if (Global.S.RedP2 == "Fox")
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? foxLose : foxWin);
        }
        else if (Global.S.RedP2 == "Hawk")
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? hawkLose : hawkWin);
        }
        else if (Global.S.RedP2 == "Baboon")
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? baboonLose : baboonWin);
        }
        else if (Global.S.RedP2 == "Fish")
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? fishLose : fishWin);
        }
        else
        {
            GameObject.Find("Player4Image").GetComponent<Image>().sprite = ((!Global.S.REDISWINRAR) ? dogeLose : dogeWin);
        }
    }
    public bool canPressStartToGo = false;

    void resetGame()
    {
        Application.LoadLevel("CharacterSelect");
    }
    public GameObject canGoText;
    void canGo()
    {
        canPressStartToGo = true;
        canGoText.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if (canPressStartToGo && ControlManager.playerPressedStart())
        {
            resetGame();
        }
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
}

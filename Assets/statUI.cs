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

    // Use this for initialization
    void Start () {
        player1Goals = GameObject.Find("Player1Goals").GetComponent<Text>();
        player1Steals = GameObject.Find("Player1Steals").GetComponent<Text>();
        player1Time = GameObject.Find("Player1Goals").GetComponent<Text>();

        player2Goals = GameObject.Find("Player2Goals").GetComponent<Text>();
        player2Steals = GameObject.Find("Player2Steals").GetComponent<Text>();
        player2Time = GameObject.Find("Player2Goals").GetComponent<Text>();

        player3Goals = GameObject.Find("Player3Goals").GetComponent<Text>();
        player3Steals = GameObject.Find("Player3Steals").GetComponent<Text>();
        player3Time = GameObject.Find("Player3Goals").GetComponent<Text>();

        player4Goals = GameObject.Find("Player4Goals").GetComponent<Text>();
        player4Steals = GameObject.Find("Player4Steals").GetComponent<Text>();
        player4Time = GameObject.Find("Player4Goals").GetComponent<Text>();


        player1Goals.text += Statistics.goalsScored[0];
        player1Steals.text += Statistics.steals[0];
        player1Time.text += Statistics.timeControlled[0];

        player2Goals.text += Statistics.goalsScored[1];
        player2Steals.text += Statistics.steals[1];
        player2Time.text += Statistics.timeControlled[1];

        player3Goals.text += Statistics.goalsScored[2];
        player3Steals.text += Statistics.steals[2];
        player3Time.text += Statistics.timeControlled[2];

        player4Goals.text += Statistics.goalsScored[3];
        player4Steals.text += Statistics.steals[3];
        player4Time.text += Statistics.timeControlled[3];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;


public class Statistics : MonoBehaviour {
    public static Statistics S;

    public GameObject[] PlayerList;
    public Player[] playerScripts = new Player[4];

    static public int[] goalsScored = new int[4];
    static public int[] steals = new int[4];

    void Awake()
    {
        S = this;
        DontDestroyOnLoad(transform.gameObject);
        Initialize();
        ResetStatistics();
    }

    public void Initialize()
    {
        GameObject[] pUnordered = GameObject.FindGameObjectsWithTag("Player");
        PlayerList = new GameObject[4];
        for (int c = 0; c < pUnordered.Length; ++c)
        {
            Player temp = pUnordered[c].GetComponent<Player>();
            if (temp.my_number == 0)
            {
                PlayerList[0] = pUnordered[c];
            }
            else if (temp.my_number == 1)
            {
                PlayerList[1] = pUnordered[c];
            }
            else if (temp.my_number == 2)
            {
                PlayerList[2] = pUnordered[c];
            }
            else
            {
                PlayerList[3] = pUnordered[c];
            }
            playerScripts[c] = temp;
        }
    }

    public void ResetStatistics()
    {
        for(int i = 0; i < PlayerList.Length; i++)
        {
            goalsScored[i] = 0;
            steals[i] = 0;
        }
    }

    public void PrintStatistics()
    {
        for (int i = 0; i < PlayerList.Length; i++)
        {
            print(PlayerList[i].name + " " + goalsScored[i] + " goals, " + steals[i] + " steals");
        }
    }

    public void goalStat(int playerNum)
    {
        goalsScored[playerNum]++;
    }

    public void stealStat(int playerNum)
    {
        steals[playerNum]++;
    }
}

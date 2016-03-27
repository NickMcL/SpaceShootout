using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {
    public Text P1CharName, P1CharDescrip, P2CharName, P2CharDescrip, TitleText;
    public Image flash;
    public GameObject inputModule, inputModule2;

    public Dictionary<string, string> Descriptions = new Dictionary<string, string>();


    Color flashcolor;
	// Use this for initialization
	void Start () {
        inputModule2.SetActive(false);
        Descriptions["Madoka"] = "Magical Girl, can cast ranged spell every 5 seconds";
        Descriptions["Goku"] = "High Strength, can gain temporary buff to kick speed";
        Descriptions["Freeza"] = "High Speed, temporary speed boost comes up more often.";
        Descriptions["Prince"] = "Good at singing, can stun the enemy goalie with his singing for 2 seconds";
        Descriptions["Cage"] = "Balanced Character, all around bit higher than average speed and power";
        Descriptions["Doge"] = "No bonuses but can play as doge";
        flashcolor = flash.color;
        flashcolor.a = 0f;
        flash.color = flashcolor;
	}

    public int PlayerSelecting = 1;

    public string HoveredChar = "";

    public void HoverOverCharacter(string Character)
    {
        HoveredChar = Character;
        if(PlayerSelecting == 1)
        {
            P1CharName.text = Character;
            P1CharDescrip.text = Descriptions[Character];
        } else
        {
            P2CharName.text = Character;
            P2CharDescrip.text = Descriptions[Character];
        }
    }

    IEnumerator Flash()
    {
        flashcolor.a = 1f;
        flash.color = flashcolor;
        for(int c = 0; c < 5; ++c)
        {
            yield return new WaitForSeconds(0.1f);
            flashcolor.a -= .2f;
            flash.color = flashcolor;
        }

        flashcolor.a = 0f;
        flash.color = flashcolor;
    }

    public void ChooseCharacter()
    {
        StartCoroutine(Flash());
        if(PlayerSelecting == 2)
        {
            Global.S.P2Character = HoveredChar;
            // load next level
        } else
        {
            Global.S.P1Character = HoveredChar;
            ++PlayerSelecting;
            TitleText.text = "Player 2: Choose Your Character!";
            inputModule.SetActive(false);
            inputModule2.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

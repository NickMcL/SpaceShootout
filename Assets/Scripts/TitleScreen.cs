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
        Descriptions["Bear"] = "Increased kicking power.";
        Descriptions["Fish"] = "Shoots bubble projectile that can move the ball.";
        Descriptions["Hawk"] = "High Speed, temporary speed boost comes up more often.";
        Descriptions["Baboon"] = "Can stun the other player at close range.";
        Descriptions["Fox"] = "Balanced chracter, slightly increased kicking power and speed.";
        Descriptions["Elephant"] = "Larger size, less bouncy.";
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

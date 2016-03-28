using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {
    public Text P1CharName, P1CharDescrip, P2CharName, P2CharDescrip, TitleText;
    public Image flash;
    public GameObject inputModule, inputModule2;

    public Image spriteP1, spriteP2;

    public Dictionary<string, string> Descriptions = new Dictionary<string, string>();

    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;

    public string nextSceneName;

    Color flashcolor;
	// Use this for initialization
	void Start () {
        PlaySound("Spacearray", 1f);
        inputModule2.SetActive(false);
        Descriptions["Bear"] = "Increased kicking power.";
        Descriptions["Fish"] = "Shoots bubble projectile that can move the ball.";
        Descriptions["Hawk"] = "High Speed, temporary speed boost comes up more often.";
        Descriptions["Baboon"] = "Can stun the other player at close range.";
        Descriptions["Fox"] = "Balanced chracter, slightly increased kicking power and speed.";
        Descriptions["Dog"] = "No noticable abilities but get to play as doge.";
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
            if(Character == "Bear")
            {
                spriteP1.sprite = bearsprite;
            } else if (Character == "Fish")
            {
                spriteP1.sprite = fishprite;
            } else if (Character == "Hawk")
            {
                spriteP1.sprite = hawksprite;
            } else if (Character == "Baboon")
            {
                spriteP1.sprite = baboonsprite;
            } else if (Character == "Fox")
            {
                spriteP1.sprite = foxsprite;
            } else
            {
                spriteP1.sprite = dogsprite;
            }
        } else
        {
            P2CharName.text = Character;
            P2CharDescrip.text = Descriptions[Character]; if (Character == "Bear")
            {
                spriteP2.sprite = bearsprite;
            }
            else if (Character == "Fish")
            {
                spriteP2.sprite = fishprite;
            }
            else if (Character == "Hawk")
            {
                spriteP2.sprite = hawksprite;
            }
            else if (Character == "Baboon")
            {
                spriteP2.sprite = baboonsprite;
            }
            else if (Character == "Fox")
            {
                spriteP2.sprite = foxsprite;
            }
            else
            {
                spriteP2.sprite = dogsprite;
            }
        }
    }

    IEnumerator Flash()
    {
        PlaySound("select01", 1f);

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

            Application.LoadLevel(nextSceneName);

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

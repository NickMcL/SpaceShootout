using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {
    Color RED_COLOR = new Color(1f, 0.7f, 0.7f);
    Color BLUE_COLOR = new Color(0.7f, 0.7f, 1f);

    public Text P1CharName, P1CharDescrip, P2CharName, P2CharDescrip, TitleText, P3CharName, P3CharDescrip, P4CharName, P4CharDescrip;
    public Image flash;
    public GameObject inputModule, inputModule2, inputModule3, inputModule4;

    public Image spriteP1, spriteP2, spriteP3, spriteP4;

    public Dictionary<string, string> Descriptions = new Dictionary<string, string>();

    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;

    public string nextSceneName;

    Color flashcolor;
    // Use this for initialization
    void Start() {
        PlaySound("Spacearray", 1f);
        inputModule2.SetActive(false);
        Descriptions["Bear"] = "Bigger in size, better at bodyblocking.";
        Descriptions["Fish"] = "Improved shooting speed.";
        Descriptions["Hawk"] = "Higher speed.";
        Descriptions["Baboon"] = "Tackles opponents harder.";
        Descriptions["Fox"] = "Slightly immproved speed and shoot power.";
        Descriptions["Dog"] = "No noticable abilities but has extremely high Luck.";
        flashcolor = flash.color;
        flashcolor.a = 0f;
        flash.color = flashcolor;
    }

    public int PlayerSelecting = 1;

    public string HoveredChar = "";

    public void HoverOverCharacter(string Character) {
        HoveredChar = Character;
        if (PlayerSelecting == 1) {
            P1CharName.text = Character;
            P1CharDescrip.text = Descriptions[Character];
            if (Character == "Bear") {
                spriteP1.sprite = bearsprite;
            } else if (Character == "Fish") {
                spriteP1.sprite = fishprite;
            } else if (Character == "Hawk") {
                spriteP1.sprite = hawksprite;
            } else if (Character == "Baboon") {
                spriteP1.sprite = baboonsprite;
            } else if (Character == "Fox") {
                spriteP1.sprite = foxsprite;
            } else {
                spriteP1.sprite = dogsprite;
            }
            spriteP1.color = BLUE_COLOR;
        } else if (PlayerSelecting == 2) {
            P2CharName.text = Character;
            P2CharDescrip.text = Descriptions[Character];
            if (Character == "Bear") {
                spriteP2.sprite = bearsprite;
            } else if (Character == "Fish") {
                spriteP2.sprite = fishprite;
            } else if (Character == "Hawk") {
                spriteP2.sprite = hawksprite;
            } else if (Character == "Baboon") {
                spriteP2.sprite = baboonsprite;
            } else if (Character == "Fox") {
                spriteP2.sprite = foxsprite;
            } else {
                spriteP2.sprite = dogsprite;
            }
            spriteP2.color = BLUE_COLOR;
        } else if (PlayerSelecting == 3) {

            P3CharName.text = Character;

            spriteP3.color = Color.white;
            P3CharDescrip.text = Descriptions[Character]; if (Character == "Bear") {
                spriteP3.sprite = bearsprite;
            } else if (Character == "Fish") {
                spriteP3.sprite = fishprite;
            } else if (Character == "Hawk") {
                spriteP3.sprite = hawksprite;
            } else if (Character == "Baboon") {
                spriteP3.sprite = baboonsprite;
            } else if (Character == "Fox") {
                spriteP3.sprite = foxsprite;
            } else {
                spriteP3.sprite = dogsprite;
            }
            spriteP3.color = RED_COLOR;
        } else {
            P4CharName.text = Character;

            spriteP4.color = Color.white;
            P4CharDescrip.text = Descriptions[Character]; if (Character == "Bear") {
                spriteP4.sprite = bearsprite;
            } else if (Character == "Fish") {
                spriteP4.sprite = fishprite;
            } else if (Character == "Hawk") {
                spriteP4.sprite = hawksprite;
            } else if (Character == "Baboon") {
                spriteP4.sprite = baboonsprite;
            } else if (Character == "Fox") {
                spriteP4.sprite = foxsprite;
            } else {
                spriteP4.sprite = dogsprite;
            }
            spriteP4.color = RED_COLOR;
        }
    }

    IEnumerator Flash() {
        PlaySound("select01", 1f);

        flashcolor.a = 1f;
        flash.color = flashcolor;
        for (int c = 0; c < 5; ++c) {
            yield return new WaitForSeconds(0.1f);
            flashcolor.a -= .2f;
            flash.color = flashcolor;
        }

        flashcolor.a = 0f;
        flash.color = flashcolor;
    }

    public void ChooseCharacter() {
        HoverOverCharacter(HoveredChar);
        StartCoroutine(Flash());
        if (PlayerSelecting == 2) {
            // blue second player
            Global.S.BlueP2 = HoveredChar;
            TitleText.text = "Player 3: Choose Your Character!";
        } else if (PlayerSelecting == 1) {
            // blue first player
            Global.S.BlueP1 = HoveredChar;
            TitleText.text = "Player 2: Choose Your Character!";
        } else if (PlayerSelecting == 3) {
            Global.S.RedP1 = HoveredChar;
            TitleText.text = "Player 4: Choose Your Character!";
        } else if (PlayerSelecting == 4) {
            Global.S.RedP2 = HoveredChar;
            TitleText.text = "Get ready for the game!";
            Invoke("startGame", 2.0f);
        }
        ++PlayerSelecting;
    }

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

    void startGame() {
        Application.LoadLevel(nextSceneName);
    }

    // Update is called once per frame
    void Update() {

    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {
    public static TitleScreen S;

    Color RED_COLOR = new Color(1f, 0.7f, 0.7f);
    Color BLUE_COLOR = new Color(0.7f, 0.7f, 1f);
    Color NO_COLOR = new Color(0f, 0f, 0f, 0f);

    public Text P1CharName, P1CharDescrip, P2CharName, P2CharDescrip, TitleText, P3CharName, P3CharDescrip, P4CharName, P4CharDescrip;
    public Image flash;
    public GameObject inputModule, inputModule2, inputModule3, inputModule4;

    public Image spriteP1, spriteP2, spriteP3, spriteP4;

    public Dictionary<string, string> Descriptions = new Dictionary<string, string>();

    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;

    public string nextSceneName;
    public bool everyone_picked = false;

    Color flashcolor;
    public Image CONTROLS;
    bool canPressStartToGo = false;
    int selected_players;

    bool[] character_chosen = new bool[ControlManager.TOTAL_PLAYERS];
    string[] hover_character = new string[ControlManager.TOTAL_PLAYERS];

    void Awake() {
        S = this;
    }

    // Use this for initialization
    void Start() {
        PlaySound("Spacearray", 1f);
        inputModule2.SetActive(false);
        Descriptions["Bear"] = "Bigger in size; better at bodyblocking.";
        Descriptions["Fish"] = "Fastest shot speed.";
        Descriptions["Hawk"] = "Highest movement speed.";
        Descriptions["Baboon"] = "Highest tackling power.";
        Descriptions["Fox"] = "Slightly improved movement speed and shot power.";
        Descriptions["Dog"] = "No noticable abilities but has extremely high luck.";
        flashcolor = flash.color;
        flashcolor.a = 0f;
        flash.color = flashcolor;
        selected_players = 0;

        for (int i = 0; i < character_chosen.Length; ++i) {
            character_chosen[i] = false;
        }


        PlaySound("choose your character", 1f);
    }

    public void hoverOverCharacter(int player_num, string character) {
        if (character_chosen[player_num]) {
            return;
        }

        hover_character[player_num] = character;
        if (player_num == 0) {
            P1CharName.text = character;
            P1CharDescrip.text = Descriptions[character];
            if (character == "Bear") {
                spriteP1.sprite = bearsprite;
            } else if (character == "Fish") {
                spriteP1.sprite = fishprite;
            } else if (character == "Hawk") {
                spriteP1.sprite = hawksprite;
            } else if (character == "Baboon") {
                spriteP1.sprite = baboonsprite;
            } else if (character == "Fox") {
                spriteP1.sprite = foxsprite;
            } else {
                spriteP1.sprite = dogsprite;
            }
            spriteP1.color = BLUE_COLOR;
        } else if (player_num == 1) {
            P2CharName.text = character;
            P2CharDescrip.text = Descriptions[character];
            if (character == "Bear") {
                spriteP2.sprite = bearsprite;
            } else if (character == "Fish") {
                spriteP2.sprite = fishprite;
            } else if (character == "Hawk") {
                spriteP2.sprite = hawksprite;
            } else if (character == "Baboon") {
                spriteP2.sprite = baboonsprite;
            } else if (character == "Fox") {
                spriteP2.sprite = foxsprite;
            } else {
                spriteP2.sprite = dogsprite;
            }
            spriteP2.color = BLUE_COLOR;
        } else if (player_num == 2) {

            P3CharName.text = character;

            spriteP3.color = Color.white;
            P3CharDescrip.text = Descriptions[character];
            if (character == "Bear") {
                spriteP3.sprite = bearsprite;
            } else if (character == "Fish") {
                spriteP3.sprite = fishprite;
            } else if (character == "Hawk") {
                spriteP3.sprite = hawksprite;
            } else if (character == "Baboon") {
                spriteP3.sprite = baboonsprite;
            } else if (character == "Fox") {
                spriteP3.sprite = foxsprite;
            } else {
                spriteP3.sprite = dogsprite;
            }
            spriteP3.color = RED_COLOR;
        } else {
            P4CharName.text = character;

            spriteP4.color = Color.white;
            P4CharDescrip.text = Descriptions[character];
            if (character == "Bear") {
                spriteP4.sprite = bearsprite;
            } else if (character == "Fish") {
                spriteP4.sprite = fishprite;
            } else if (character == "Hawk") {
                spriteP4.sprite = hawksprite;
            } else if (character == "Baboon") {
                spriteP4.sprite = baboonsprite;
            } else if (character == "Fox") {
                spriteP4.sprite = foxsprite;
            } else {
                spriteP4.sprite = dogsprite;
            }
            spriteP4.color = RED_COLOR;
        }
    }

    public void stopHoverOverCharacter(int player_num) {
        if (character_chosen[player_num]) {
            return;
        }

        if (player_num == 0) {
            P1CharName.text = "";
            P1CharDescrip.text = "";
            spriteP1.color = NO_COLOR;
        } else if (player_num == 1) {
            P2CharName.text = "";
            P2CharDescrip.text = "";
            spriteP2.color = NO_COLOR;
        } else if (player_num == 2) {
            P3CharName.text = "";
            P3CharDescrip.text = "";
            spriteP3.color = NO_COLOR;
        } else if (player_num == 3) {
            P4CharName.text = "";
            P4CharDescrip.text = "";
            spriteP4.color = NO_COLOR;
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

    public void chooseCharacter(int player_num) {
        if (hover_character[player_num] == "Bear") {
            PlaySound("bear", 1f);
        } else if (hover_character[player_num] == "Fish") {
            PlaySound("fish", 1f);
        } else if (hover_character[player_num] == "Hawk") {
            PlaySound("hawk", 1f);
        } else if (hover_character[player_num] == "Baboon") {
            PlaySound("baboon", 1f);
        } else if (hover_character[player_num] == "Fox") {
            PlaySound("fox", 1f);
        } else {
            PlaySound("doge", 1f);
        }

        StartCoroutine(Flash());
        if (player_num == 0) {
            Global.S.BlueP1 = hover_character[player_num];
        } else if (player_num == 1) {
            Global.S.BlueP2 = hover_character[player_num];
        } else if (player_num == 2) {
            Global.S.RedP1 = hover_character[player_num];
        } else if (player_num == 3) {
            Global.S.RedP2 = hover_character[player_num];
        }

        ++selected_players;
        character_chosen[player_num] = true;
        if (selected_players == ControlManager.TOTAL_PLAYERS) {
            TitleText.text = "Get ready for the game!";
            CONTROLS.raycastTarget = true;
            everyone_picked = true;
            Invoke("ShowControls", 3.0f);
        }
    }

    public void unchooseCharacter(int player_num) {
        if (canPressStartToGo) {
            return;
        }
        PlaySound("undo");
        --selected_players;
        character_chosen[player_num] = false;
        stopHoverOverCharacter(player_num);
    }

    void ShowControls() {
        PlaySound("get ready", 1f);
        Color c = CONTROLS.color;
        c.a = 1f;
        CONTROLS.color = c;
        canPressStartToGo = true;
        //Invoke("CanPressStart", 1f);
    }

    void CanPressStart() {
        canPressStartToGo = true;
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
        if (canPressStartToGo && ControlManager.playerPressedStart()) {
            startGame();
        }
    }
}

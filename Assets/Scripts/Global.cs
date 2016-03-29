using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
    public static Global S;

    public string BlueP1, BlueP2;
    public string RedP1, RedP2;

    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;
    public Color BlueColor, RedColor;

    // Use this for initialization
    void Awake() {
        S = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ControlManager.use_controllers = !ControlManager.use_controllers;
        }
    }

    public void loadSprites() {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        for (int c = 0; c < g.Length; ++c) {
            Player p = g[c].GetComponent<Player>();
            SpriteRenderer sr = g[c].GetComponent<SpriteRenderer>();
            if (p.my_number == 0) {
                p.RedTeam = false;
                sr.color = BlueColor;
                if (BlueP1 == "Bear") {
                    sr.sprite = bearsprite;
                } else if (BlueP1 == "Fish") {
                    sr.sprite = fishprite;
                } else if (BlueP1 == "Hawk") {
                    sr.sprite = hawksprite;
                } else if (BlueP1 == "Baboon") {
                    sr.sprite = baboonsprite;
                } else if (BlueP1 == "Fox") {
                    sr.sprite = foxsprite;
                } else {
                    sr.sprite = dogsprite;
                }
            } else if (p.my_number == 1) {
                p.RedTeam = false;

                sr.color = BlueColor;
                if (BlueP2 == "Bear") {
                    sr.sprite = bearsprite;
                } else if (BlueP2 == "Fish") {
                    sr.sprite = fishprite;
                } else if (BlueP2 == "Hawk") {
                    sr.sprite = hawksprite;
                } else if (BlueP2 == "Baboon") {
                    sr.sprite = baboonsprite;
                } else if (BlueP2 == "Fox") {
                    sr.sprite = foxsprite;
                } else {
                    sr.sprite = dogsprite;
                }
            } else if (p.my_number == 2) {
                p.RedTeam = true;
                sr.color = RedColor;
                if (RedP1 == "Bear") {
                    sr.sprite = bearsprite;
                } else if (RedP1 == "Fish") {
                    sr.sprite = fishprite;
                } else if (RedP1 == "Hawk") {
                    sr.sprite = hawksprite;
                } else if (RedP1 == "Baboon") {
                    sr.sprite = baboonsprite;
                } else if (RedP1 == "Fox") {
                    sr.sprite = foxsprite;
                } else {
                    sr.sprite = dogsprite;
                }
            } else {
                p.RedTeam = true;
                sr.color = RedColor;
                if (RedP2 == "Bear") {
                    sr.sprite = bearsprite;
                } else if (RedP2 == "Fish") {
                    sr.sprite = fishprite;
                } else if (RedP2 == "Hawk") {
                    sr.sprite = hawksprite;
                } else if (RedP2 == "Baboon") {
                    sr.sprite = baboonsprite;
                } else if (RedP2 == "Fox") {
                    sr.sprite = foxsprite;
                } else {
                    sr.sprite = dogsprite;
                }
            }
        }
    }

    public void score(bool red) {
        CameraShaker.S.DoShake(0.08f, 0.15f);
        if (red) {
            HUD.S.RedTeamScored();
        } else {
            HUD.S.BlueTeamScored();
        }
    }
}

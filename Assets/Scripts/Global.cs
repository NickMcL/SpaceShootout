using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
    public static Global S;

    public string BlueP1, BlueP2;
    public string RedP1, RedP2;


    public bool REDISWINRAR = false, TIE = false;
    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;
    public Color BlueColor, RedColor;

    // Use this for initialization
    void Awake() {
        S = this;
        DontDestroyOnLoad(this.gameObject);
        ControlManager.initControllers();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ControlManager.use_controllers = !ControlManager.use_controllers;
        }
    }

    public float bearRadius = 0.4f;
    public float fishBoost = 1.4f;
    public float hawkBoost = 1.4f;
    public float foxBoost = 1.2f;
    public float baboonBoost = 1.5f;


    public void SetBearAttributes(Player p) {
        p.GetComponent<CircleCollider2D>().radius = bearRadius;
    }
    public void SetFishAttributes(Player p) {
        p.shot_force *= fishBoost;
    }
    public void SetHawkAttributes(Player p) {
        p.acceleration *= hawkBoost;
    }
    public void SetBaboonAttributes(Player p) {
        p.isBaboon = true;
        p.pushSpeed *= baboonBoost;
    }
    public void SetFoxAttributes(Player p) {
        p.shot_force *= foxBoost;
        p.acceleration *= foxBoost;
    }
    public void SetDogeAttributes(Player p) {
        p.isDoge = true;
    }
    public void loadSprites() {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        for (int c = 0; c < g.Length; ++c) {
            Player p = g[c].GetComponent<Player>();
            SpriteRenderer sr = g[c].GetComponent<SpriteRenderer>();
            if (p.my_number == 0) {
                sr.color = BlueColor;
                if (BlueP1 == "Bear") {
                    sr.sprite = bearsprite;
                    SetBearAttributes(p);
                } else if (BlueP1 == "Fish") {
                    sr.sprite = fishprite;
                    SetFishAttributes(p);
                } else if (BlueP1 == "Hawk") {
                    sr.sprite = hawksprite;
                    SetHawkAttributes(p);
                } else if (BlueP1 == "Baboon") {
                    sr.sprite = baboonsprite;
                    SetBaboonAttributes(p);
                } else if (BlueP1 == "Fox") {
                    sr.sprite = foxsprite;
                    SetFoxAttributes(p);
                } else {
                    sr.sprite = dogsprite;
                    SetDogeAttributes(p);
                }
            } else if (p.my_number == 1) {
                sr.color = BlueColor;
                if (BlueP2 == "Bear") {
                    sr.sprite = bearsprite;
                    SetBearAttributes(p);
                } else if (BlueP2 == "Fish") {
                    sr.sprite = fishprite;
                    SetFishAttributes(p);
                } else if (BlueP2 == "Hawk") {
                    sr.sprite = hawksprite;
                    SetHawkAttributes(p);
                } else if (BlueP2 == "Baboon") {
                    sr.sprite = baboonsprite;
                    SetBaboonAttributes(p);
                } else if (BlueP2 == "Fox") {
                    sr.sprite = foxsprite;
                    SetFoxAttributes(p);
                } else {
                    sr.sprite = dogsprite;
                    SetDogeAttributes(p);
                }
            } else if (p.my_number == 2) {
                sr.color = RedColor;
                if (RedP1 == "Bear") {
                    sr.sprite = bearsprite;
                    SetBearAttributes(p);
                } else if (RedP1 == "Fish") {
                    sr.sprite = fishprite;
                    SetFishAttributes(p);
                } else if (RedP1 == "Hawk") {
                    sr.sprite = hawksprite;
                    SetHawkAttributes(p);
                } else if (RedP1 == "Baboon") {
                    sr.sprite = baboonsprite;
                    SetBaboonAttributes(p);
                } else if (RedP1 == "Fox") {
                    sr.sprite = foxsprite;
                    SetFoxAttributes(p);
                } else {
                    sr.sprite = dogsprite;
                    SetDogeAttributes(p);
                }
            } else {
                sr.color = RedColor;
                if (RedP2 == "Bear") {
                    sr.sprite = bearsprite;
                    SetBearAttributes(p);
                } else if (RedP2 == "Fish") {
                    sr.sprite = fishprite;
                    SetFishAttributes(p);
                } else if (RedP2 == "Hawk") {
                    sr.sprite = hawksprite;
                    SetHawkAttributes(p);
                } else if (RedP2 == "Baboon") {
                    sr.sprite = baboonsprite;
                    SetBaboonAttributes(p);
                } else if (RedP2 == "Fox") {
                    sr.sprite = foxsprite;
                    SetFoxAttributes(p);
                } else {
                    sr.sprite = dogsprite;
                    SetDogeAttributes(p);
                }
            }
        }
    }

    public void score(HUD.Team team) {
        CameraShaker.S.DoShake(0.08f, 0.15f);
        HUD.S.teamScored(team);
    }
}

using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour
{
    public static Global S;

    public string P1Character;
    public string P2Character;


    public Sprite dogsprite, bearsprite, fishprite, hawksprite, baboonsprite, foxsprite;


    // Use this for initialization
    void Awake()
    {
        S = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLevelWasLoaded(int level)
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        for(int c = 0; c < g.Length; ++c)
        {
            Player p = g[c].GetComponent<Player>();
            SpriteRenderer sr = g[c].GetComponent<SpriteRenderer>();
            if(p.my_number == 1)
            {
                if (P1Character == "Bear")
                {
                    sr.sprite = bearsprite;
                }
                else if (P1Character == "Fish")
                {
                    sr.sprite = fishprite;
                }
                else if (P1Character == "Hawk")
                {
                    sr.sprite = hawksprite;
                }
                else if (P1Character == "Baboon")
                {
                    sr.sprite = baboonsprite;
                }
                else if (P1Character == "Fox")
                {
                    sr.sprite = foxsprite;
                }
                else
                {
                    sr.sprite = dogsprite;
                }
            } else
            {
                if (P2Character == "Bear")
                {
                    sr.sprite = bearsprite;
                }
                else if (P2Character == "Fish")
                {
                    sr.sprite = fishprite;
                }
                else if (P2Character == "Hawk")
                {
                    sr.sprite = hawksprite;
                }
                else if (P2Character == "Baboon")
                {
                    sr.sprite = baboonsprite;
                }
                else if (P2Character == "Fox")
                {
                    sr.sprite = foxsprite;
                }
                else
                {
                    sr.sprite = dogsprite;
                }
            }
        }
    }

    public void score()
    {
        CameraShaker.S.DoShake(0.08f, 0.15f);
        if (HUD.S.player2isGoalie) {
            HUD.S.Player1Scored();
        } else {
            HUD.S.Player2Scored();
        }
    }
}

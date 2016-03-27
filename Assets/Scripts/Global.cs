using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
    public static Global S;

    public string P1Character;
    public string P2Character;
	// Use this for initialization
	void Awake () {
        S = this;
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

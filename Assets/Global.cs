using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
    public static Global S;
	// Use this for initialization
	void Awake () {
        S = this;
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

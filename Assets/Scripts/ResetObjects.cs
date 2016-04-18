using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetObjects : MonoBehaviour
{
    public static ResetObjects S;

    public GameObject AsteroidBreakablePrefab;
    public GameObject[] ObjectsToReset;
    public List<Vector3> OriginalPositions;
    public List<Vector3> OriginalScales;

    // Use this for initialization
    void Awake()
    {
        S = this;

        ObjectsToReset = GameObject.FindGameObjectsWithTag("AsteroidBreakable");

        foreach (GameObject obj in ObjectsToReset)
        {
            OriginalPositions.Add(obj.transform.position);
            OriginalScales.Add(obj.transform.localScale);
        }
    }

    public void Reset()
    {
        GameObject[] AsteroidBreakable = GameObject.FindGameObjectsWithTag("AsteroidBreakable");
        for (int i = 0; i < AsteroidBreakable.Length; i++)
        {
            Destroy(AsteroidBreakable[i]);
        }

        GameObject[] AsteroidMinis = GameObject.FindGameObjectsWithTag("AsteroidMini");
        for (int i = 0; i < AsteroidMinis.Length; i++)
        {
            Destroy(AsteroidMinis[i]);
        }

        for (int i = 0; i < ObjectsToReset.Length; i++)
        {
            GameObject astroid = Instantiate(AsteroidBreakablePrefab, OriginalPositions[i], Quaternion.identity) as GameObject;
            astroid.transform.localScale = OriginalScales[i];
        }

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in Players)
        {
            player.transform.GetChild(1).gameObject.SetActive(false);
        }

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.transform.GetChild(1).gameObject.SetActive(true);

    }

}

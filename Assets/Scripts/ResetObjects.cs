using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetObjects : MonoBehaviour
{
    public static ResetObjects S;

    public GameObject AsteroidBreakablePrefab;
    public GameObject[] ObjectsToReset;
    public List<Vector3> OriginalPositions;

    // Use this for initialization
    void Awake()
    {
        S = this;

        ObjectsToReset = GameObject.FindGameObjectsWithTag("AsteroidBreakable");

        foreach (GameObject obj in ObjectsToReset)
        {
            OriginalPositions.Add(obj.transform.position);
        }
    }

    public void Reset()
    {
        for(int i = 0; i < ObjectsToReset.Length; i++)
        {
            GameObject astroid = Instantiate(AsteroidBreakablePrefab, OriginalPositions[i], Quaternion.identity) as GameObject;
            //ObjectsToReset[i].transform.position = OriginalPositions[i];
        }

        GameObject[] AsteroidMinis = GameObject.FindGameObjectsWithTag("AsteroidMini");
        for (var i = 0; i < AsteroidMinis.Length; i++)
        {
            Destroy(AsteroidMinis[i]);
        }
    }

}

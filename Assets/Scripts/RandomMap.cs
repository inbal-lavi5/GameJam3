using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMap : MonoBehaviour
{
    // [SerializeField] private GameObject[] objectsToSpread;
    private String[] objectsToSpreadString = new []{"Stop", "Stop", "Stop", "Stop", "Stop",  "Stop", "Stop", "Stop", "Stop", "Stop", "Speed","Time", "Bomb"};

    void Start()
    {
        int i = Random.Range(0, objectsToSpreadString.Length);
        GameObject itemToSpreadLoad = (GameObject) Resources.Load(objectsToSpreadString[i], typeof(GameObject));
        Instantiate(itemToSpreadLoad, transform.position, Quaternion.identity);
    }
}

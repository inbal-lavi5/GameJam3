using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMap : MonoBehaviour
{
    // [SerializeField] private GameObject[] objectsToSpread;
    private List<String> objectsToSpreadString = new List<string>
    {
        "Speed", "Speed", "Speed", "Speed",
        "Bomb", "Bomb", "Bomb",
        "Time", "Time"
    };

    void Start()
    {
        int positionsCount = transform.childCount;
        int alreadyHave = objectsToSpreadString.Count;
        for (int j = alreadyHave; j < positionsCount; j++)
        {
            objectsToSpreadString.Add("Stop");
        }
        
        for (int i = 0; i < positionsCount; i++)
        {
            int index = Random.Range(0, objectsToSpreadString.Count);
            GameObject itemToSpreadLoad = (GameObject) Resources.Load(objectsToSpreadString[index], typeof(GameObject));
            Instantiate(itemToSpreadLoad, transform.GetChild(i).position, Quaternion.identity);
            objectsToSpreadString.RemoveAt(index);
        }
    }
}
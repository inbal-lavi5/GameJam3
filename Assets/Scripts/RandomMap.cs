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
        print("alreadyHave " + alreadyHave);
        for (int j = alreadyHave; j < positionsCount; j++)
        {
            objectsToSpreadString.Add("Stop");
        }

        print(objectsToSpreadString.Count + " " + positionsCount);

        for (int i = 0; i < positionsCount; i++)
        {
            int index = Random.Range(0, objectsToSpreadString.Count);
            print(index);
            GameObject itemToSpreadLoad = (GameObject) Resources.Load(objectsToSpreadString[index], typeof(GameObject));
            Instantiate(itemToSpreadLoad, transform.GetChild(i).position, Quaternion.identity);
            objectsToSpreadString.RemoveAt(index);
        }

        print(objectsToSpreadString.Count + " " + positionsCount);
    }
}
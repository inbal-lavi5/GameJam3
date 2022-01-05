using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public int pickUpsToSpreadAtStart = 50;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;

    private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};

    // Start is called before the first frame update
    void Awake()
    {
        // print("levels: " + levelsList[0] + " " + levelsList[1]);

        InstantiatePickups();
    }

    private void InstantiatePickups()
    {
        for (int i = 0; i < pickUpsToSpreadAtStart; i++)
        {
            GameObject pickup = Instantiate(Resources.Load("PickUp")) as GameObject;

            float posx = Random.Range(-60f, 60f);
            float posz = Random.Range(-200f, 1200f);
            pickup.transform.position = new Vector3(posx, 2.6f, posz);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    public void Reset()
    {
        level = 0;
        SceneManager.LoadScene(levelsList[0]);
        InstantiatePickups();
    }

    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(levelsList[level]);
        InstantiatePickups();
    }
}
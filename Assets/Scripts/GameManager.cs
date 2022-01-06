using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public int MANA_MAX = 100;
    public int MANA_MIN = 0;
    public float MANA_DEC = 30f;
    public float MANA_ADD = 30f;


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

    public void InstantiatePickups()
    {
        print("INSTTTTTTTTTTTTTTT");
        GameObject load = (GameObject) Resources.Load("PickUp", typeof(GameObject));
        print(load == null);
        for (int i = 0; i < pickUpsToSpreadAtStart; i++)
        {
            GameObject pickup = Instantiate(load);

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
        StartCoroutine(ExecuteAfterSceneLoaded());
        // InstantiatePickups();
    }


    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(levelsList[level]);
        StartCoroutine(ExecuteAfterSceneLoaded());
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelsList[level]));
        // InstantiatePickups();
    }

    IEnumerator ExecuteAfterSceneLoaded()
    {
        bool isLoaded = SceneManager.GetActiveScene().isLoaded;
        // SceneManager.sceneUnloaded
        yield return new WaitUntil(() => isLoaded);
        InstantiatePickups();
    }
}
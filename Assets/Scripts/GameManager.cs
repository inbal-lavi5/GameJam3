using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public int MANA_MAX = 200;
    public int MANA_MIN = 0;
    public float MANA_DEC = 30f;
    public float MANA_ADD = 30f;

    [SerializeField] public int manaPickUpsToSpreadAtStart = 20;
    [SerializeField] public int pickUpsToSpreadAtEnd = 20;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;

    private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};

    [SerializeField] private int currentGoal = 0;
    [SerializeField] private int maxGoal = 3;
    [SerializeField] public List<string> goals;

    [SerializeField] public List<string> tagsList = new List<string>
        {"Car", "Building", "Store", "StreetLight", "Tree"};

    // Start is called before the first frame update
    void Awake()
    {
        InstantiatePickups("PickUpMana");
        ResetGoals();
    }

    private void ResetGoals()
    {
        goals = new List<string>();
        for (int i = 0; i < maxGoal; i++)
        {
            int randomGoal = Random.Range(0, tagsList.Count);
            goals.Add(tagsList[randomGoal]);
        }
    }

    public void InstantiatePickups(string pickupname)
    {
        GameObject load = (GameObject) Resources.Load(pickupname, typeof(GameObject));
        for (int i = 0; i < pickUpsToSpreadAtEnd; i++)
        {
            GameObject pickup = Instantiate(load);

            float posx = Random.Range(-160f, 160f);
            float posz = Random.Range(-200f, 400f);
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
        ResetGoals();
        currentGoal = 0;
    }

    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(levelsList[level]);
        StartCoroutine(ExecuteAfterSceneLoaded());
    }

    IEnumerator ExecuteAfterSceneLoaded()
    {
        bool isLoaded = SceneManager.GetActiveScene().isLoaded;
        // SceneManager.sceneUnloaded
        yield return new WaitUntil(() => isLoaded);
        InstantiatePickups("PickUpMana");
    }

    public string getGoalsAsString()
    {
        string textBox3 = "";
        for (var i = 0; i < goals.Count; i++)
        {
            if (i == currentGoal)
            {
                textBox3 += "**" + goals[i] + "** | ";
            }
            else
            {
                textBox3 += goals[i] + " | ";
            }
        }

        // textBox3 += "curr: " + currentGoal;

        return textBox3;
    }

    public void NextGoal()
    {
        currentGoal++;
    }

    public void AddGoal(CustomTag tags)
    {
        if (WinCondition())
        {
            InstantiatePickups("PickUp");
            return;
        }

        if (tags.HasTag(goals[currentGoal]))
        {
            NextGoal();
        }
        // else
        // {
        //     Reset();
        // }
    }

    public bool WinCondition()
    {
        return currentGoal >= maxGoal;
    }
}
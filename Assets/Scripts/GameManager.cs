using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour // Singleton<GameManager>
{
    [SerializeField] public int manaPickUpsToSpreadAtStart = 20;
    [SerializeField] public int pickUpsToSpreadAtEnd = 20;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;

    private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};

    [SerializeField] private int currentToDestroy = 0;
    [SerializeField] private int numOfObjectsToDestroy = 3;
    [SerializeField] public List<string> toDestroy;

    [SerializeField] public Text objectToDestroyText;
    [SerializeField] public Image objectToDestroy;
    [SerializeField] public List<Sprite> images;

    [SerializeField] public List<string> optionsToDestroyList = new List<string>
        {"Car", "Building", "Store", "StreetLight", "Tree"};

    // Start is called before the first frame update
    void Awake()
    {
        InstantiatePickups("PickUpMana");
        ResetItemsToDestroy();
    }

    private void ResetItemsToDestroy()
    {
        toDestroy = new List<string>();
        for (int i = 0; i < numOfObjectsToDestroy; i++)
        {
            int randomGoal = Random.Range(0, optionsToDestroyList.Count);
            toDestroy.Add(optionsToDestroyList[randomGoal]);
        }
        objectToDestroyText.text = getItemsToDestroyAsString();
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
        ResetItemsToDestroy();
        currentToDestroy = 0;
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

    public string getItemsToDestroyAsString()
    {
        if (WinCondition())
        {
            return "";
        }
        string textBox3 = "";
        for (var i = 0; i < toDestroy.Count; i++)
        {
            if (i == currentToDestroy)
            {
                textBox3 += "**" + toDestroy[i] + "** | ";
            }
            else
            {
                textBox3 += toDestroy[i] + " | ";
            }
        }

        // textBox3 += "curr: " + currentGoal;

        return textBox3;
    }

    public void NextItemToDestroy()
    {
        currentToDestroy++;
    }

    /**
     * gets a destroyed item and checks if its the right item to destroy
     */
    public bool AddDestroyedItem(CustomTag tags)
    {
        if (WinCondition())
        {
            InstantiatePickups("PickUp");
            return false;
        }

        if (tags.HasTag(toDestroy[currentToDestroy]))
        {
            NextItemToDestroy();
            objectToDestroyText.text = getItemsToDestroyAsString();
            return true;
        }
        else
        {
            // Reset();
            return false;
        }
    }

    public bool WinCondition()
    {
        return currentToDestroy >= numOfObjectsToDestroy;
    }
}
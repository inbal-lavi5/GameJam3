using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour // Singleton<GameManager>
{
    [SerializeField] public int manaPickUpsToSpreadAtStart = 20;
    [SerializeField] public int pickUpsToSpreadAtEnd = 20;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;

    private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};

    private int destroyedItemsCounter = 0;
    [SerializeField] private int numOfObjectsToDestroy = 3;

    [SerializeField] public Image objectToDestroy;
    [SerializeField] public List<Sprite> images;
    
    
    void Awake()
    {
        InstantiatePickups("PickUpMana");
        ResetItemsToDestroy();
    }

    private void ResetItemsToDestroy()
    {
        int randomGoal = Random.Range(0, images.Count);
        objectToDestroy.sprite = images[randomGoal];
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
        destroyedItemsCounter = 0;
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


    public void NextItemToDestroy()
    {
        destroyedItemsCounter++;
        ResetItemsToDestroy();

        if (WinCondition())
        {
            InstantiatePickups("PickUp");
            objectToDestroy.gameObject.SetActive(false);
        }
    }

    /**
     * gets a destroyed item and checks if its the right item to destroy
     */
    public bool AddDestroyedItem(CustomTag tags)
    {
        if (WinCondition())
        {
            return false;
        }

        if (tags.HasTag(objectToDestroy.sprite.name))
        {
            NextItemToDestroy();
            return true;
        }
        return false;
    }

    public bool WinCondition()
    {
        return destroyedItemsCounter >= numOfObjectsToDestroy;
    }
}
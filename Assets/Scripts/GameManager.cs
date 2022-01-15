using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int manaPickUpsToSpreadAtStart = 30;
    [SerializeField] public int pickUpsToSpread = 15;
    [SerializeField] public SoundManager SoundManager;
    [SerializeField] private int level = 1;
    [SerializeField] public List<string> levelsList = new List<string>
        {"city tutorial", "city", "country"};

    
    void Awake()
    {
        spreadItems("Time", -160, 160, -260, 400, pickUpsToSpread);
        spreadItems("Stop", -160, 160, -260, 400, 20);
        spreadItems("Speed", -160, 160, -260, 400, manaPickUpsToSpreadAtStart);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //todo remove at end
        {
            Reset();
        }
    }

    
    public void Reset()
    {
        level = 1;
        SceneManager.LoadScene(levelsList[level]);
    }

    
    public void spreadItems(String item, float xMin, float xMax, float zMin, float zMax, int amount)
    {
        GameObject itemToSpreadLoad = (GameObject) Resources.Load(item, typeof(GameObject));

        for (int i = 0; i < amount; i++)
        {
            GameObject itemToSpreadInstance = Instantiate(itemToSpreadLoad);
            itemToSpreadInstance.transform.position = generateLocation(xMin, xMax, zMin, zMax);
        }
    }

    
    private Vector3 generateLocation(float xMin, float xMax, float zMin, float zMax)
    {
        float posx = Random.Range(xMin, xMax);
        float posz = Random.Range(zMin, zMax);
        return new Vector3(posx, 4f, posz);
    }

    
    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(levelsList[level]);
        //StartCoroutine(ExecuteAfterSceneLoaded());
    }

    IEnumerator ExecuteAfterSceneLoaded()
    {
        bool isLoaded = SceneManager.GetActiveScene().isLoaded;
        // SceneManager.sceneUnloaded
        yield return new WaitUntil(() => isLoaded);
        // InstantiatePickups("PickUpMana", manaPickUpsToSpreadAtStart);
    }
    
    
    public void PlaySound(SoundManager.Sounds sfx)
    {
        SoundManager.PlaySound(sfx);
    }
}
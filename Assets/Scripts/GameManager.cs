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
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScreenEffectsManager screenManager;
    
    [SerializeField] private GameObject losePanel;

    private int timeToRemovePart = 10;
    [SerializeField] public int powerUpsTime;

    [SerializeField] private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};


    void Awake()
    {
        // power ups
        spreadItems("Time", -160, 160, -260, 400, 10);
        spreadItems("Speed", -160, 160, -260, 400, 10);
        spreadItems("Bomb", -160, 160, -260, 400, 5);

        // power down
        spreadItems("Stop", -160, 160, -260, 400, 20);
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
        SceneManager.LoadScene(levelsList[level]);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
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


    public void AddRigid(GameObject otherGameObject)
    {
        otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f),
            Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
    }

    public void AddRigidChildren(Transform parent)
    {
        parent.tag = "Collapsed";

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                // child.gameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f),
                //     Random.Range(0f, 0.5f));
                AddRigid(child.gameObject);
                child.tag = "Collapsed";
                // StartCoroutine(RemovePart(child));
                Destroy(child.gameObject, timeToRemovePart);
            }

            if (child.childCount > 0)
            {
                AddRigidChildren(child);
            }
        }
    }

    public void PlaySound(SoundManager.Sounds sfx)
    {
        soundManager.PlaySound(sfx);
    }

    public void ManageScreen(ScreenEffectsManager.Effects fx)
    {
        screenManager.ScreenEffect(fx, powerUpsTime);
    }
}
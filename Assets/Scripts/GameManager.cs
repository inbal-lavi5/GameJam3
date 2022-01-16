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
    [SerializeField] private GameObject nextPanel;

    private int timeToRemovePart = 10;
    [SerializeField] public int powerUpsTime;

    [SerializeField] private int boundaryXmin = -190;
    [SerializeField] private int boundaryXmax = 190;
    [SerializeField] private int boundaryZmin = -200;
    [SerializeField] private int boundaryZmax = 400;


    void Awake()
    {
        // power ups
        spreadItems("Time", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 10);
        spreadItems("Speed", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 10);
        spreadItems("Bomb", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 5);

        // power down
        spreadItems("Stop", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 20);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }


    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevelScreen()
    {
        nextPanel.SetActive(true);
    }

    public void LoseScreen()
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
        // level++;
        // SceneManager.LoadScene(levelsList[level]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
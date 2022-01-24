using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private ScreenEffectsManager screenManager;

    [SerializeField] private GameObject loadingScene;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject losePanelFirstButton;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject winPanelFirstButton;
    [SerializeField] private GameObject nextPanel;
    [SerializeField] private GameObject nextPanelFirstButton;

    private int timeToRemovePart = 15;
    [SerializeField] public int powerUpsTime;

    /*[SerializeField] private int boundaryXmin = -190;
    [SerializeField] private int boundaryXmax = 190;
    [SerializeField] private int boundaryZmin = -200;
    [SerializeField] private int boundaryZmax = 400;*/

    private int numOfLevels = 3;

    void Awake()
    {
        /*// power ups
        spreadItems("Time", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 10);
        spreadItems("Speed", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 10);
        spreadItems("Bomb", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 5);

        // power down
        spreadItems("Stop", boundaryXmin, boundaryXmax, boundaryZmin, boundaryZmax, 20);*/
    }

    /*public void spreadItems(String item, float xMin, float xMax, float zMin, float zMax, int amount)
    {
        GameObject itemToSpreadLoad = (GameObject) Resources.Load(item, typeof(GameObject));

        for (int i = 0; i < amount; i++)
        {
            GameObject itemToSpreadInstance = Instantiate(itemToSpreadLoad);
            itemToSpreadInstance.transform.position = generateLocation(xMin, xMax, zMin, zMax);
        }
    }*/

    /*private Vector3 generateLocation(float xMin, float xMax, float zMin, float zMax)
    {
        float posx = Random.Range(xMin, xMax);
        float posz = Random.Range(zMin, zMax);
        return new Vector3(posx, 4f, posz);
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }


    public void ResetGame()
    {
        StartCoroutine(LoadAsync(0));
    }

    public void ResetLevel()
    {
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextLevelScreen()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= numOfLevels)
        {
            winPanel.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>()
                .SetSelectedGameObject(winPanelFirstButton);
        }
        else
        {
            nextPanel.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>()
                .SetSelectedGameObject(nextPanelFirstButton);
        }
    }

    public void LoseScreen()
    {
        losePanel.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>()
            .SetSelectedGameObject(losePanelFirstButton);
    }

    public void NextLevel()
    {
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        
        loadingScene.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressSlider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
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
                AddRigid(child.gameObject);
                child.tag = "Collapsed";
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
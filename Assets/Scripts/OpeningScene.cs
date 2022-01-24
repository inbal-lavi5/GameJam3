using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    [SerializeField] private GameObject loadingScene;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;
    [SerializeField] private GameObject playButton;

    void Start()
    {
        StartCoroutine(SelectButton());
    }

    IEnumerator SelectButton()
    {
        yield return new WaitForSeconds(0.7f);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
    }

    public void quitGame()
    {
        Application.Quit();
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
}
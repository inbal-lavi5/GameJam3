using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    public static bool isPaused;
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuFirstButton;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        gameManager.PlaySound(SoundManager.Sounds.PAUSE);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(pauseMenuFirstButton);
    }

    public void Resume()
    {
        gameManager.PlaySound(SoundManager.Sounds.PLAY);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}

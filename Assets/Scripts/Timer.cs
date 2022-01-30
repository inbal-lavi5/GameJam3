using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public Text timerText;
    private GameManager gameManager;

    private float currentTime = 0f;
    private float startingTime = 60f;
    private float timeToAdd = 20;
    private float scaleTimeFactor = 1;
    private int ogFontSize;

    private bool stop;
    private bool startCountDown = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentTime = startingTime;
        ogFontSize = timerText.fontSize;
    }

    void Update()
    {
        if (stop) return;
        currentTime -= 1 * Time.deltaTime * scaleTimeFactor;
        timerText.text = currentTime.ToString("0");

        if (currentTime <= 5)
        {
            timerText.color = Color.red;
            timerText.fontSize = ogFontSize + 40;

            if (!startCountDown)
            {
                startCountDown = true;
                gameManager.PlaySound(SoundManager.Sounds.TIMER);
            }
        }
        
        else
        {
            timerText.color = Color.white;
            timerText.fontSize = ogFontSize;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
        }
    }

    private void startOver()
    {
        // continue
    }

    public void scaleTimeUp()
    {
        scaleTimeFactor = 10;
    }

    public void scaleTimeNormal()
    {
        scaleTimeFactor = 1;
    }

    public void addTime()
    {
        currentTime += timeToAdd;
    }

    public bool isFinished()
    {
        return currentTime <= 0;
    }

    public void stopTimer()
    {
        stop = true;
    }
}
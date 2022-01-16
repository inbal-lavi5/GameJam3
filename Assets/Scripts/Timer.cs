using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public Text timerText;

    private float currentTime = 0f;
    private float startingTime = 60f;
    private float timeToAdd = 5;
    private float scaleTimeFactor = 1;
    private int ogFontSize;

    void Start()
    {
        currentTime = startingTime;
        ogFontSize = timerText.fontSize;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime * scaleTimeFactor;
        timerText.text = currentTime.ToString("0");

        if (currentTime <= 10)
        {
            timerText.color = Color.red;
            timerText.fontSize = ogFontSize + 40;
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
}
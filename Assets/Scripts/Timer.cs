using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public Text timerText;
    [SerializeField] public PlayerControl player;

    private float currentTime = 0f;
    private float startingTime = 60f;
    private float timeToAdd = 5;
    private float scaleTimeFactor = 1;
    
    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime * scaleTimeFactor;
        timerText.text = currentTime.ToString("0");

        if (currentTime <= 0)
        {
            currentTime = 0;
            player.NextLevel(); // todo change to certain condition
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
}

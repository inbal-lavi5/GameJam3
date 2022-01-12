using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [SerializeField] public Text timerText;
    private float currentTime = 0f;
    private float startingTime = 60f;
    public PlayerControl player;
    
    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        timerText.text = currentTime.ToString("0");

        if (currentTime <= 0)
        {
            currentTime = 0;
            player.NextLevel();
        }
    }

    private void startOver()
    {
        // continue
    }

    public void decTime()
    {
        currentTime -= 10;
    }
}

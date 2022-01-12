using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [SerializeField] public Text timerText;
    private float currentTime = 0f;
    private float startingTime = 60f;
    
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
            startOver();
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

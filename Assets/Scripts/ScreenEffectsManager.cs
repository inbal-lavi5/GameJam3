using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectsManager : MonoBehaviour
{
    public enum Effects
    {
        NORMAL,
        TIME,
        STOP,
        SPEED,
        BOMB
    }

    [SerializeField] private GameObject stopScreen;
    [SerializeField] private GameObject speedScreen;
    [SerializeField] private GameObject timeScreen;

    private void Start()
    {
        normal();
    }

    public void ScreenEffect(Effects sfx)
    {
        switch (sfx)
        {
            case Effects.TIME:
                timedOn();
                break;
            case Effects.STOP:
                blockOn();
                break;
            case Effects.SPEED:
                speedOn();
                break;
            case Effects.BOMB:
                bombOn();
                break;
            case Effects.NORMAL:
                normal();
                break;
        }
    }

    public void speedOn()
    {
        speedScreen.SetActive(true);
    }

    public void timedOn()
    {
        StartCoroutine(timeCoroutine());
    }

    public void blockOn()
    {
        stopScreen.SetActive(true);
    }

    public void bombOn()
    {
        StartCoroutine(bombCoroutine());
    }

    public void normal()
    {
        stopScreen.SetActive(false);
        speedScreen.SetActive(false);
        timeScreen.SetActive(false);
    }

    IEnumerator timeCoroutine()
    {
        timeScreen.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        timeScreen.SetActive(false);
    }

    IEnumerator bombCoroutine()
    {
        speedScreen.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        speedScreen.SetActive(false);
    }
}
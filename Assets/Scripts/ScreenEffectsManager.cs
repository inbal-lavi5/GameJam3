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

    public void ScreenEffect(Effects sfx, float time)
    {
        switch (sfx)
        {
            case Effects.TIME:
                StartCoroutine(timeCoroutine());
                break;
            case Effects.STOP:
                StartCoroutine(stopCoroutine(time));
                break;
            case Effects.SPEED:
                StartCoroutine(speedCoroutine(time));
                break;
            case Effects.BOMB:
                StartCoroutine(bombCoroutine());
                break;
            case Effects.NORMAL:
                normal();
                break;
        }
    }

    public void normal()
    {
        stopScreen.SetActive(false);
        speedScreen.SetActive(false);
        timeScreen.SetActive(false);
    }

    IEnumerator stopCoroutine(float time)
    {
        stopScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        stopScreen.SetActive(false);
    }

    IEnumerator speedCoroutine(float time)
    {
        speedScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        speedScreen.SetActive(false);
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
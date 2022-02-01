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
    [SerializeField] private GameObject explosionScreen;
    [SerializeField] private Animator fxAnimator;

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
        fxAnimator.Play("stop", 0, 0);
        stopScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        stopScreen.SetActive(false);
    }

    IEnumerator speedCoroutine(float time)
    {
        fxAnimator.Play("speed", 0, 0);
        speedScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        speedScreen.SetActive(false);
    }

    IEnumerator timeCoroutine()
    {
        fxAnimator.Play("time", 0, 0);
        timeScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        timeScreen.SetActive(false);
    }

    IEnumerator bombCoroutine()
    {
        fxAnimator.Play("blast", 0, 0);
        explosionScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        explosionScreen.SetActive(false);
    }
}
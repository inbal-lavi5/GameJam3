using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject green;

    private void Start()
    {
        normal();
    }

    public void speedOn()
    {
        red.SetActive(true);
    }
    
    public void timedOn()
    {
        StartCoroutine(timeCoroutine());
    }
    
    public void blockOn()
    {
        blue.SetActive(true);
    }

    public void normal()
    {
        blue.SetActive(false);
        red.SetActive(false);
        green.SetActive(false);
    }
    
    IEnumerator timeCoroutine()
    {
        green.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        green.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    [SerializeField] public GameObject boom0Left;
    [SerializeField] public GameObject boom0Right;
    
    [SerializeField] public GameObject boom1Left;
    [SerializeField] public GameObject boom1Right;
    
    [SerializeField] public GameObject boom2Left;
    [SerializeField] public GameObject boom2Right;
    
    private int curMode = 0;

    private void Start()
    {
        setMode(curMode);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch (curMode)
            {
                case 0:
                    curMode = 1;
                    break;
                case 1:
                    curMode = 2;
                    break;
                case 2:
                    curMode = 0;
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (curMode)
            {
                case 0:
                    curMode = 2;
                    break;
                case 1:
                    curMode = 0;
                    break;
                case 2:
                    curMode = 1;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (curMode)
            {
                case 0:
                    SceneManager.LoadScene("village");
                    break;
                case 1:
                    SceneManager.LoadScene("city tutorial");
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }
        
        setMode(curMode);

    }

    private void setMode(int curPos)
    {
        switch (curPos)
        {
            case 0:
                boom0Left.SetActive(true);
                boom0Right.SetActive(true);
                boom1Left.SetActive(false);
                boom1Right.SetActive(false);
                boom2Left.SetActive(false);
                boom2Right.SetActive(false);
                break;

            case 1:
                boom0Left.SetActive(false);
                boom0Right.SetActive(false);
                boom1Left.SetActive(true);
                boom1Right.SetActive(true);
                boom2Left.SetActive(false);
                boom2Right.SetActive(false);
                break;
            
            case 2:
                boom0Left.SetActive(false);
                boom0Right.SetActive(false);
                boom1Left.SetActive(false);
                boom1Right.SetActive(false);
                boom2Left.SetActive(true);
                boom2Right.SetActive(true);
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    
    private int EXP_MAX = 100;
    private int EXP_MIN = 0;
    private float EXP_ADD = 1f;
    
    private Image bar;
    private Text precentage;
    private float curAmount;

    
    private void Start()
    {
        bar = GetComponent<Image>();
        precentage = transform.GetChild(0).GetComponent<Text>();
        curAmount = EXP_MIN;
        updateUI();
    }
    

    public void addExp()
    {
        curAmount += EXP_ADD;

        if (curAmount > EXP_MAX)
        {
            curAmount = EXP_MAX;
        }
        
        updateUI();
    }

    
    private void updateUI()
    {
        precentage.text = curAmount + "%";
        bar.fillAmount = normalizedMana();
    }


    private float normalizedMana()
    {
        return curAmount / EXP_MAX;
    }
}
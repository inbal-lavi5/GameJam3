using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    
    public int MANA_MAX = 100;
    public int MANA_MIN = 0;
    public float MANA_ADD = 1f;
    
    private Image bar;
    private Text count;
    private float manaAmount;

    
    private void Start()
    {
        bar = GetComponent<Image>();
        count = transform.GetChild(0).GetComponent<Text>();
        manaAmount = MANA_MIN;
    }
    

    public void addMana()
    {
        manaAmount += MANA_ADD;

        if (manaAmount > MANA_MAX)
        {
            manaAmount = MANA_MAX;
        }
        
        updateUI();
    }

    
    private void updateUI()
    {
        count.text = manaAmount + "%";
        bar.fillAmount = normalizedMana();
    }


    private float normalizedMana()
    {
        return manaAmount / MANA_MAX;
    }
}
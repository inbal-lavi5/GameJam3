using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image bar;

    public int MANA_MAX = 200;
    public int MANA_MIN = 0;
    public float MANA_DEC = 30f;
    public float MANA_ADD = 30f;
    
    private float manaAmount;
    public bool dec;

    private void Start()
    {
        bar = GetComponent<Image>();
        manaAmount = MANA_MAX;
    }

    private void Update()
    {
        if (dec)
        {
            decMana();
        }

        // else
        // {
        //     addMana();
        // }

        bar.fillAmount = normalizedMana();
    }


    public void addMana()
    {
        // manaAmount += MANA_ADD * Time.deltaTime;
        manaAmount += 40;

        if (manaAmount > MANA_MAX)
        {
            manaAmount = MANA_MAX;
        }
    }

    public void decManaBeMaca()
    {
        manaAmount -= 20;

        if (manaAmount < MANA_MIN)
        {
            manaAmount = MANA_MIN;
        }
    }
    private void decMana()
    {
        manaAmount -= MANA_DEC * Time.deltaTime;

        if (manaAmount < MANA_MIN)
        {
            manaAmount = MANA_MIN;
        }
    }

    private float normalizedMana()
    {
        return manaAmount / MANA_MAX;
    }

    public float getMana()
    {
        return manaAmount;
    }

    public bool isManaFinished()
    {
        return manaAmount <= MANA_MIN;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image bar;
    private GameManager gameManager;

    private float manaAmount;
    public bool dec;

    private void Start()
    {
        bar = GetComponent<Image>();
        gameManager = GameManager.Instance;
        manaAmount = gameManager.MANA_MAX;
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
        // manaAmount += gameManager.MANA_ADD * Time.deltaTime;
        manaAmount += 40;

        if (manaAmount > gameManager.MANA_MAX)
        {
            manaAmount = gameManager.MANA_MAX;
        }
    }

    private void decMana()
    {
        manaAmount -= gameManager.MANA_DEC * Time.deltaTime;

        if (manaAmount < gameManager.MANA_MIN)
        {
            manaAmount = gameManager.MANA_MIN;
        }
    }

    private float normalizedMana()
    {
        return manaAmount / gameManager.MANA_MAX;
    }

    public float getMana()
    {
        return manaAmount;
    }
}
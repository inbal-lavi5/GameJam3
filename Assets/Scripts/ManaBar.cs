using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image bar;
    private Text count;

    public int MANA_MAX = 100;
    public int MANA_MIN = 0;
    public float MANA_DEC = 30f;
    public float MANA_ADD = 1f;
    public float addBeMaca = 40f;
    public float decBeMaca = 20f;

    private float manaAmount;
    public bool dec;

    private void Start()
    {
        bar = GetComponent<Image>();
        count = transform.GetChild(0).GetComponent<Text>();
        manaAmount = MANA_MIN;
    }

    private void Update()
    {
        /*if (dec)
        {
            decMana();
        }

        else
        {
            addMana();
        }*/

        count.text = getMana() + "%";  //  + "/" + pickUpsToCollectTillExplosion;
        bar.fillAmount = normalizedMana();
    }


    public void addManaBeMaca()
    {
        // manaAmount += MANA_ADD * Time.deltaTime;
        manaAmount += addBeMaca;

        if (manaAmount > MANA_MAX)
        {
            manaAmount = MANA_MAX;
        }
    }

    public void decManaBeMaca()
    {
        manaAmount -= decBeMaca;

        if (manaAmount < MANA_MIN)
        {
            manaAmount = MANA_MIN;
        }
    }

    public void addMana()
    {
        manaAmount += MANA_ADD; // * Time.deltaTime;

        if (manaAmount > MANA_MAX)
        {
            manaAmount = MANA_MAX;
        }
    }

    private void decMana()
    {
        manaAmount -= MANA_DEC; // * Time.deltaTime;

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
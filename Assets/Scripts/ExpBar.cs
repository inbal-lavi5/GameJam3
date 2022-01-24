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

    private float curAmount;
    private Image bar;
    [SerializeField] private Text precentage;


    private void Start()
    {
        bar = GetComponent<Image>();
        // precentage = transform.GetChild(0).GetComponent<Text>();
        curAmount = EXP_MIN;
        updateUI();
    }


    public void addExp(float size)
    {
        // curAmount += EXP_ADD;
        curAmount += size;

        if (curAmount >= EXP_MAX)
        {
            curAmount = EXP_MAX;
        }

        updateUI();
    }


    private void updateUI()
    {
        precentage.text = curAmount + "%";
        bar.fillAmount = normalizedMana();
        float clamp = RemapClamped(normalizedMana(), 0, 1, -420, 420);
        precentage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, clamp);
    }


    private float normalizedMana()
    {
        return curAmount / EXP_MAX;
    }

    public static float RemapClamped(float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
    {
        float t = (aValue - aIn1) / (aIn2 - aIn1);
        if (t > 1f)
            return aOut2;
        if(t < 0f)
            return aOut1;
        return aOut1 + (aOut2 - aOut1) * t;
    }

    public bool isFinished()
    {
        return curAmount >= EXP_MAX;
    }
}
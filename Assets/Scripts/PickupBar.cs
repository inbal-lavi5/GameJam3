using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBar : MonoBehaviour
{
    private Image bar;
    private GameManager gameManager;

    private float pickUpAmount = 0;

    private void Start()
    {
        bar = GetComponent<Image>();
        gameManager = GameManager.Instance;
        bar.fillAmount = normalizedPickup();
    }

    // private void Update()
    // {
    //     bar.fillAmount = normalizedPickup();
    // }


    public void addPickup()
    {
        pickUpAmount++;
        if (pickUpAmount > gameManager.MANA_MAX)
        {
            pickUpAmount = gameManager.MANA_MAX;
        }
        bar.fillAmount = normalizedPickup();
    }
    

    private float normalizedPickup()
    {
        return pickUpAmount / gameManager.pickUpsToCollectTillExplosion;
    }
}
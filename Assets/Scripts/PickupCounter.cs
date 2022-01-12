using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupCounter : MonoBehaviour
{
    private Text count;
    private float pickUpAmount = 0;
    // private int pickUpsToCollectTillExplosion = 10;

    private void Awake()
    {
        count = GetComponent<Text>();
        UpdateText();
    }

    public void SetUpMax(int max)
    {
        // pickUpsToCollectTillExplosion = max;
        UpdateText();
    }

    private void UpdateText()
    {
        count.text = "Score: " + pickUpAmount;  //  + "/" + pickUpsToCollectTillExplosion;
    }

    public void AddPickup()
    {
        pickUpAmount += 10;
        UpdateText();
    }

    public bool CollectedAll()
    {
        return true; // todo change or delete
        // return pickUpAmount >= pickUpsToCollectTillExplosion;
    }


    /*
    private float normalizedPickup()
    {
        return pickUpAmount / pickUpsToCollectTillExplosion;
    }*/
}
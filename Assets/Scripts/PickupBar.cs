using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBar : MonoBehaviour
{
    private Image bar;
    private float pickUpAmount = 0;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;

    private void Start()
    {
        bar = GetComponent<Image>();
        bar.fillAmount = normalizedPickup();
    }

    public void addPickup()
    {
        pickUpAmount++;
        bar.fillAmount = normalizedPickup();
    }

    private float normalizedPickup()
    {
        return pickUpAmount / pickUpsToCollectTillExplosion;
    }
}
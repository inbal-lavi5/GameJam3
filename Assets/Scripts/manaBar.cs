using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manaBar : MonoBehaviour
{
   private Image bar;
   private GameManager gameManager;
   
   private float manaAmount = 0;
   
   private void Start()
   {
      bar = GetComponent<Image>();
      gameManager = GameManager.Instance;
   }

   private void Update()
   {
      if (Input.GetKey(KeyCode.Space))
      {
         decMana();
      }

      else
      {
         addMana();
      }
      
      bar.fillAmount = normalizedMana();
      Debug.Log(manaAmount);
   }


   private void addMana()
   {
      manaAmount += gameManager.MANA_ADD * Time.deltaTime;

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
}

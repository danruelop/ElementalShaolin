﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {

    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int numberOfElements;
    public int award;
    public float maxMana = 10;
    public float currentMana;
    public Boolean manaShieldObtained;

   

    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd.isKey)
        {
            numberOfKeys++;
        }
        else if(itemToAdd.isElement && numberOfElements < 4)
        {
            numberOfElements++;
        }
        else if (itemToAdd.isAward)
        {
            award++;
        } 
        else if(itemToAdd.isManaShield)
        {
            manaShieldObtained = true;
        }
        else
        {
            if (!items.Contains(itemToAdd))
            {
                items.Add(itemToAdd);
            }
        }
    }



}

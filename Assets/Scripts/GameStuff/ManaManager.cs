using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{

    public Slider manaSlider;
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        manaSlider.maxValue = playerInventory.maxMana;
        manaSlider.value = playerInventory.maxMana;
        playerInventory.currentMana = playerInventory.maxMana;
    }

    // Update is called once per frame
    

    public void decreaseMana()
    {
        manaSlider.value -= 1;
        playerInventory.currentMana -= 1;
        if (manaSlider.value < 0)
        {
            manaSlider.value = 0;
            playerInventory.currentMana = 0;
        }
    }
}

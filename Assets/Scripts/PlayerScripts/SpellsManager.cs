using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsManager : MonoBehaviour
{
    public Image[] spells;
    public Inventory playerInventory;
    public int numberOfElements_comp = 0;
    


    // Start is called before the first frame update
    

     // Update is called once per frame
    private void Update()
    {
        if(playerInventory.numberOfElements != numberOfElements_comp)
        {
            numberOfElements_comp = playerInventory.numberOfElements;
            spells[playerInventory.numberOfElements - 1].gameObject.SetActive(true);
        }
        
    }
    




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsManager : MonoBehaviour
{
    public Image[] spells;
    public Inventory playerInventory;
    


    // Start is called before the first frame update
    

     // Update is called once per frame
    private void Update()
    {
       
        if(playerInventory.numberOfElements == 1)
        {
            spells[0].gameObject.SetActive(true);
        }

        if (playerInventory.numberOfElements == 2)
        {
            spells[0].gameObject.SetActive(true);
            spells[1].gameObject.SetActive(true);
        }
        if (playerInventory.numberOfElements == 3)
        {
            spells[0].gameObject.SetActive(true);
            spells[1].gameObject.SetActive(true);
            spells[2].gameObject.SetActive(true);
        }
        if (playerInventory.numberOfElements == 4)
        {
            spells[0].gameObject.SetActive(true);
            spells[1].gameObject.SetActive(true);
            spells[2].gameObject.SetActive(true);
            spells[3].gameObject.SetActive(true);
        }
    }
    




}

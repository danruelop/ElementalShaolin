using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleSpellManager : MonoBehaviour
{
    public Inventory playerInventory;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (playerInventory.numberOfElements > 0 && this.gameObject.name == "ObstacleFirstRoomEarth")
        {
            this.gameObject.SetActive(false);
        }

        if (playerInventory.numberOfElements > 1 && this.gameObject.name == "ObstacleFirstRoomWater")
        {
            this.gameObject.SetActive(false);
        }

        if (playerInventory.numberOfElements > 2 && this.gameObject.name == "ObstacleFirstRoomAir")
        {
            this.gameObject.SetActive(false);
        }

        if (playerInventory.numberOfElements > 3 && this.gameObject.name == "ObstacleFirstRoomFire")
        {
            this.gameObject.SetActive(false);

        }
    }
    }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public GameObject health_100;
    public GameObject health_75;
    public GameObject health_50;
    public GameObject health_25;
    public FloatValue healthContainer;
    public FloatValue playerCurrentHealth;

    void Start()
    {
        InitHealthBar();
    }

    // Update is called once per frame
    public void InitHealthBar()
    {
       
        health_100.gameObject.SetActive(true);
        
    }

    public void UpdateHealth()
    {
        int vidas = 4;
      while(vidas > 0)
      {

        
        if(playerCurrentHealth.initialValue == 3)
        {
           
            health_100.gameObject.SetActive(false);
            health_75.gameObject.SetActive(true);
                vidas--;

        } else if (playerCurrentHealth.initialValue == 2)
        {
            health_75.gameObject.SetActive(false);
            health_50.gameObject.SetActive(true);
                vidas--;
            } else if (playerCurrentHealth.initialValue == 1)
        {
            health_50.gameObject.SetActive(false);
            health_25.gameObject.SetActive(true);
                vidas--;
            }
        }
    }


}

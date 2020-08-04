using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;

    void Start()
    {
        InitHearts();
    }

    // Update is called once per frame
    public void InitHearts()
    {
       
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
        
    }

    public void UpdateHealth()
    {
        float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            if (i <= tempHealth - 1) {
                hearts[i].sprite = fullHeart;
            
            } else if (i >= tempHealth)
            {
                hearts[i].sprite = emptyHeart;
            } else
            {
                hearts[i].sprite = halfHeart;
            }
        }
               




    }


}

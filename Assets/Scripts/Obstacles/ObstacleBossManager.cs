using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBossManager : MonoBehaviour
{
    public GameObject finalBoss;
    public GameObject obstacle;
    

    
    void Update()
    {
        if (!finalBoss.activeSelf)
        {
            obstacle.SetActive(false);
        }
    }
}

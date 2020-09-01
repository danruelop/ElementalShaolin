using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMusic : MonoBehaviour
{
    public GameObject roomSound;
    public GameObject finalRoom;

    
    void Update()
    {
        if (finalRoom.gameObject.activeSelf)
        {
            roomSound.gameObject.GetComponent<AudioSource>().Stop(); 
        }
    }
}

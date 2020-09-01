using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenu : MonoBehaviour
{
    public GameObject roomSound;
    public GameObject finalMenuGO;

    void Update()
    {
        if (finalMenuGO.activeSelf)
        {
            roomSound.gameObject.GetComponent<AudioSource>().Stop();
            Time.timeScale = 0;
       
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown("escape"))
            {
                Application.Quit();
            }
        }
    }
}

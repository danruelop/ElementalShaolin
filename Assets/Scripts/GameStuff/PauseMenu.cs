using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelGO;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape") && !panelGO.activeSelf)
        {
            panelGO.SetActive(true);
            Time.timeScale = 0;
        }
        else if (panelGO.activeSelf)
        {
            if (Input.GetKeyDown("space"))
            {
                panelGO.SetActive(false);
                Time.timeScale = 1;
            }
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

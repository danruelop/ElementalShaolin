using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenu : MonoBehaviour
{
    public GameObject finalMenuGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (finalMenuGO.activeSelf)
        {
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

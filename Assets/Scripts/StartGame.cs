using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory.numberOfElements = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("EarthLevel", LoadSceneMode.Single);
        }
    }
}

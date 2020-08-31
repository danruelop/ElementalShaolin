using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Inventory playerInventory;

    private AudioSource musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        playerInventory.numberOfElements = 0;
        playerInventory.manaShieldObtained = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            musicPlayer.Play();
            SceneManager.LoadScene("EarthLevel", LoadSceneMode.Single);
        }
    }
}

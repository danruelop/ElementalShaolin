using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    private Animator anim;
    public Inventory playerInventory;

    public AudioClip breakSound;
    private AudioSource audioPot;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioPot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Smash()
    {
        if (playerInventory.numberOfElements == 0)
        {
            anim.SetBool("smash", true);
            StartCoroutine(breakCo());
            audioPot.clip = breakSound;
            audioPot.Play();
        }
    }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Earthmaster : Interactable
{
    public bool isAwake;
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (!isAwake)
            {
                WakeupEarthmaster();
            }
            else
            {
                EarthmasterAlreadyAwake();
            }
        }

    }

    public void WakeupEarthmaster()
    {
        dialogBox.SetActive(true);
        dialogText.text = dialog;
        context.Raise();
        isAwake = true;
        anim.SetBool("wakeUp", true);
    }

    public void EarthmasterAlreadyAwake()
    {
        dialogBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isAwake)
        {
            context.Raise();
            playerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isAwake)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}

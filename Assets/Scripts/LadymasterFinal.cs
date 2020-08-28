using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadymasterFinal : Interactable
{
    public GameObject finalMenu;

    [Header("Contents")]
    public Item contents;
    public Inventory playerInventory;
    public bool isAwake;
    public BoolValue storedOpen;

    [Header("Signals and Dialog")]
    public Signal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;

    [Header("Animation")]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isAwake = storedOpen.RuntimeValue;
        if (isAwake)
        {
            anim.SetBool("opened", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (!isAwake)
            {
                StartCoroutine(WakeupLadymasterCo());
            }
            else
            {
                LadymasterAlreadyAwake();
            }
        }

    }

    public IEnumerator WakeupLadymasterCo()
    {
        dialogBox.SetActive(true);
        dialogText.text = contents.itemDescription;
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        raiseItem.Raise();
        context.Raise();
        isAwake = true;
        anim.SetBool("wakeUp", true);
        storedOpen.RuntimeValue = isAwake;

        yield return new WaitForSeconds(5f);
        finalMenu.SetActive(true);
    }

    public void LadymasterAlreadyAwake()
    {
        dialogBox.SetActive(false);
        raiseItem.Raise();
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

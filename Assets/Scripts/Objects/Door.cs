using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    [Header("Sound Variables")]
    public AudioClip doorSound;
    private AudioSource audioDoor;

    void Start()
    {
        audioDoor = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("interact"))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                if(playerInventory.numberOfKeys > 0)
                {
                    playerInventory.numberOfKeys--;
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        doorSprite.enabled = false;
        open = true;
        physicsCollider.enabled = false;
        audioDoor.clip = doorSound;
        audioDoor.Play();
    }

    public void close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType2
{
    key,
    enemy,
    button
}

public class DoorSwitch : Interactable
{
    [Header("Door variables")]
    public DoorType2 thisDoorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    private void Update()
    {
        if (Input.GetButtonDown("interact"))
        {
            if (playerInRange && thisDoorType == DoorType2.key)
            {
                if (playerInventory.numberOfKeys > 0)
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
    }

    public void close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
    }
}

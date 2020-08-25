using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    key,
    element,
    award
}

[CreateAssetMenu]
public class Item : ScriptableObject {

    public ItemType thisItemType;
    public Sprite itemSprite;
    public string itemDescription;
    public bool isKey;
    public bool isElement;
    public bool isAward;

}

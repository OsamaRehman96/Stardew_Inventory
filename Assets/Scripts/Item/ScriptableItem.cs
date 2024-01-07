using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="SO_Item",menuName ="Scriptable/SO-Item")]
public class ScriptableItem : ScriptableObject
{
    //ID of the item
    public int itemID;
    //The image of the item
    public Sprite visualSprite;
    //The name of the item
    public string itemName;
    //The type of item
    public ItemType itemType;

    //This bool checks if the item gets stacked 
    //in the inventory
    public bool isStackable = false;
    //max number of times this item can be stacked
    public int maxStackValue = 99;

}

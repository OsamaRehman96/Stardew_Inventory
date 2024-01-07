using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handle the similar details of the slots
public class Slot : MonoBehaviour
{
    public bool slotEmpty;
    public bool slotAvailable;

    public int stackCount = 0;
    public int maxStack = 1;

    public ItemInventory inventoryItem;

    //Initialize the item in the slot
    public void InitItemInSlot(ItemInventory _item)
    {
        inventoryItem = _item;
        maxStack = inventoryItem.SO_Item.maxStackValue;


        slotEmpty = false;
        inventoryItem.transform.SetParent(this.transform);
        inventoryItem.transform.localScale = Vector3.one;
        inventoryItem.transform.localPosition = Vector3.zero;
    }
}

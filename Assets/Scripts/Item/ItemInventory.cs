using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is used to assign items in the inventory
public class ItemInventory : Item
{

    [SerializeField] private Image itemVisual;
    //total stack of item
    public int itemCount;
   
    //Bool to check if object is stackable or not
    public bool isStackable {  get; private set; }

    //Used to Initialize inventory item in the inventory
    public void SetupInventoryItem()
    {
        itemVisual.sprite = SO_Item.visualSprite;
        gameObject.name = SO_Item.name;
        isStackable = SO_Item.isStackable;
    }


}

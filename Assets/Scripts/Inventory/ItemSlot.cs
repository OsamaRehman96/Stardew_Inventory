using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : Slot, IPointerEnterHandler
{
 

    public GameObject numberOfItems;
    public TextMeshProUGUI numberOfItemsText;
  
    private Inventory inventory;

   

    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
    }



    public void StackItem()
    {
        if (stackCount == 0)
            stackCount = 1;
    
        stackCount++;
        inventoryItem.itemCount = stackCount;
        
        if (!numberOfItems.activeSelf)
            numberOfItems.SetActive(true);


        numberOfItemsText.text = stackCount.ToString();
    }

    public void ShowItemCount(int itemCount)
    {
        stackCount = itemCount;
        numberOfItems.SetActive(true);
        numberOfItemsText.text = itemCount.ToString();
    }


    public void HideItemCount()
    {
        numberOfItems.SetActive(false);
    }


    /// <summary>
    /// Check if the stack is maxed out
    /// </summary>
    /// <returns>returns true if the the stack is maxed</returns>
    public bool GetStackMaxed()
    {
        if (stackCount == maxStack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if(inventory!=null)
        {
            inventory.hoveredItemSlot = this;
        }
     
    }

    public void RemoveItem()
    {
        inventoryItem = null;
        slotEmpty = true;
        stackCount = 1;
        maxStack = 1;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot,IPointerEnterHandler
{
    
   
    //cache for inventory
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
    }


    // Start is called before the first frame update
    void Start()
    {
        slotAvailable = true;
        slotEmpty = true;
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory != null)
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

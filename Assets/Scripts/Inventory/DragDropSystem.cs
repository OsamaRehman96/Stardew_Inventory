using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This class is responsible for the movement of items in the inventory system
public class DragDropSystem : MonoBehaviour
{
    //Set a stat
    public static DragDropSystem instance { get; private set; }
    private Inventory inventory;

    private ItemInventory draggableItem;
    private Vector2 mouseDragAnchoredPositionOffset;

    private Slot currentSlot;
    private void Awake()
    {
        instance = this;
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (draggableItem != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventory.GetItemContainer(), Input.mousePosition, null, out Vector2 targetPosition);
            targetPosition += new Vector2(-mouseDragAnchoredPositionOffset.x, -mouseDragAnchoredPositionOffset.y);

            draggableItem.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(draggableItem.GetComponent<RectTransform>().anchoredPosition, targetPosition, Time.deltaTime * 20f);
        }
    }

    public void StartedDragging(ItemInventory item)
    {
        draggableItem = item;
        draggableItem.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventory.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
        currentSlot = inventory.hoveredItemSlot;

        if (currentSlot is ItemSlot)
        {
            ItemSlot itemSlot = (ItemSlot)currentSlot;
            if (currentSlot != null)
                itemSlot.HideItemCount();
        }

        if (currentSlot is EquipmentSlot)
        {
            Debug.Log("Dragging Equipment");
        }
        // Calculate the anchored poisiton offset, where exactly on the image the player clicked
        mouseDragAnchoredPositionOffset = anchoredPosition - item.GetComponent<RectTransform>().anchoredPosition;
    }

    public void StoppedDragging()
    {
        /*ItemSlot*/
        Slot slot = inventory.hoveredItemSlot;
        Debug.LogWarning("currentSlot " + currentSlot.name);
        Debug.LogWarning(slot.name);

        bool isEquipment = (currentSlot.inventoryItem.SO_Item.itemType == ItemType.equipment) ? true : false;

        //This code checks if the slot is not empty or the same and then return the item to its orignal position
        if (currentSlot == slot || !slot.slotEmpty && slot.slotAvailable)
        {
            draggableItem.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
            draggableItem = null;
            currentSlot.inventoryItem.transform.localPosition = Vector3.zero;
            if (slot is ItemSlot)
            {
                ItemSlot itemSlot = (ItemSlot)currentSlot;

                if (itemSlot.stackCount > 1)
                {
                    itemSlot.ShowItemCount(itemSlot.stackCount);
                }
            }

        }

        //this check if slot is available and then stacks the item
        if (slot.slotEmpty && slot.slotAvailable)
        {
            if (slot is EquipmentSlot)
            {
                if (isEquipment)
                {
                    if (currentSlot != null && currentSlot is EquipmentSlot)
                    {
                        EquipmentSlot itemToRemove = (EquipmentSlot)currentSlot;
                        itemToRemove.RemoveItem();
                    }
                    else if (currentSlot != null && currentSlot is ItemSlot)
                    {
                        ItemSlot itemToRemove = (ItemSlot)currentSlot;
                        itemToRemove.RemoveItem();
                    }
                }
                else
                {
                    draggableItem.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
                    draggableItem = null;
                    currentSlot.inventoryItem.transform.localPosition = Vector3.zero;

                    if (slot is ItemSlot)
                    {
                        ItemSlot itemSlot = (ItemSlot)currentSlot;

                        if (itemSlot.stackCount > 1)
                        {
                            itemSlot.ShowItemCount(itemSlot.stackCount);
                        }
                    }
                    return;
                }
            }

            if (slot is ItemSlot)
            {
                inventory.hoveredItemSlot.stackCount = currentSlot.stackCount;
                if (currentSlot != null && currentSlot is ItemSlot)
                {
                    ItemSlot itemToRemove = (ItemSlot)currentSlot;
                    itemToRemove.RemoveItem();
                }
                else if (currentSlot != null && currentSlot is EquipmentSlot)
                {
                    EquipmentSlot itemToRemove = (EquipmentSlot)currentSlot;
                    itemToRemove.RemoveItem();
                }
                Debug.Log(inventory.hoveredItemSlot.name);

            }


            inventory.TryPlacingItem(draggableItem, /*inventory.hoveredItemSlot*/slot);
        }




        draggableItem = null;


    }
}



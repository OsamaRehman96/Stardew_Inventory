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
        ItemInventory droppableItem = draggableItem;


        bool isEquipment = (currentSlot.inventoryItem.SO_Item.itemType == ItemType.equipment) ? true : false;

        if (CheckForDropArea(draggableItem))
        {
            Debug.Log("Item Is over a dropArea");
        }
        else
        {
            //Drag the item back to its orignal position


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

                        if (currentSlot is ItemSlot)
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

        }


        //CheckForDropArea(draggableItem);
        draggableItem = null;


    }

    //Check if item is dragged over a DropArea
    private bool CheckForDropArea(ItemInventory dropItem)
    {

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Filter the raycast results to consider only the topmost UI elements
        results.Sort((r1, r2) => r1.sortingOrder.CompareTo(r2.sortingOrder));

        //I check if the first Item I hit has the dropLayer and then I try to do stuff on it
        foreach (RaycastResult result in results)
        {
            I_Dropable dropArea = result.gameObject.GetComponent<I_Dropable>();
            if (dropArea != null)
            {
                dropArea.OnItemDropped(dropItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}



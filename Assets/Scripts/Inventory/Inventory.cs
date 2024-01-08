using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using RPGM.Core;
using RPGM.Gameplay;

//Class responsible for handling inventory
public class Inventory : MonoBehaviour
{
    //List of items in inventory
    public List<ItemInventory> items;

    //List of scriptable items to keep track of stacking
    private List<ScriptableItem> SO_Items = new List<ScriptableItem>();

    //List of items slots
    public List<ItemSlot> slots;

    //Current slot the mouse is hovering over
    public Slot hoveredItemSlot { get; set; }

    //Parent object 
    [SerializeField] private Transform itemContainer;
    //item prefab to instantiate
    [SerializeField] ItemInventory inventoryItemPrefab;

    //UI overlay and panel
    [SerializeField] private Image overlay;
    [SerializeField] private Transform inventoryPanel;

    //RPGM class GameModel, to implement or change game states since Im using Unity's own 2D rpg kit for art and character input and movement
    GameModel model = Schedule.GetModel<GameModel>();

    //Dictionary to keep track of each item with its own scriptable object type
    private Dictionary<ScriptableItem, ItemInventory> itemDictionary;

    //Bool use to create a small deltay to open close inventory
    private bool canOpenClose = true;

    //Total slots available in inventory
    public int totalItemSlots = 48;
    //Slots available in the beginning
    public int availbleItemSlots = 8;

    //slot prefab to instantiate
    public GameObject itemSlotPrefab;

  


    private void Awake()
    {
        //Init function in EventManager
        EventManager.TryAddingToInventory = TryAddingItemToInventory;
        itemDictionary = new Dictionary<ScriptableItem, ItemInventory>();
    }

    private void Start()
    {
        //Initialize Inventory
        SetupInventory();
    }


    #region Inventory MAIN

    /// <summary>
    /// This function adds items to inventory and also handles stacking of items
    /// </summary>
    /// <param name="itemToAdd">Item to be added</param>
    private void TryAddingItemToInventory(Item itemToAdd)
    {
        //Check if inventory is full
        if (availbleItemSlots == items.Count)
        {
            if (itemToAdd.SO_Item.isStackable)
            {

            }
            else
            {
                Debug.LogError("Inventory is Full");
                return;
            }
        }


        //Check if slots are greatr than 0
        if (slots.Count <= 0)
            return;


        //Instantiate a new item prefab
        ItemInventory newInventoryItem = Instantiate(inventoryItemPrefab, itemContainer);
        //Assign its scriptable object type
        newInventoryItem.SO_Item = itemToAdd.SO_Item;
        //Setup inventoryItem
        newInventoryItem.SetupInventoryItem();



        //Count to keep track of loops
        int count = 0;
        //A bool that checks if we should progress to the loop or end it
        bool continueToAdd = true;

        //The first for loop checks for stackable items first. If it finds an item in the list that can be stacked then it stops the loop and stacks the item
        foreach (ItemSlot slot in slots)
        {
            //If items already exists, exit the loop
            if (!SO_Items.Contains(newInventoryItem.SO_Item))
                break;

            //If there are some items in the inventory then proceed to check those items
            if (items.Count > 0)
            {
                continueToAdd = false;
                Debug.Log("Theres an item in this list");

                //I find the slot that has the same item by going through all the slot items and checking if its maxed or not
                if (!slot.slotEmpty && newInventoryItem.isStackable && slot.inventoryItem.SO_Item == newInventoryItem.SO_Item && !slot.GetStackMaxed())
                {
                    Debug.Log("Found item in " + slot.name + " and it has " + (slot.maxStack - slot.stackCount).ToString() + " space available");
                    slot.StackItem();
                    Destroy(newInventoryItem.gameObject);
                    break;
                }
                else if (!slot.slotEmpty && newInventoryItem.isStackable && slot.inventoryItem.SO_Item == newInventoryItem.SO_Item && slot.GetStackMaxed())
                {
                    Debug.Log("Found item in " + slot.name + " and is maxed");
                    //If slot is full continue to add the item in a new slot
                    continueToAdd = true;
                    break;
                }
                //Check if the item is not stackable then add it to an available slot
                else if(!newInventoryItem.isStackable)
                {
                    //continue to add the item to slot
                    continueToAdd = true;
                    break;
                }

            }
        }

        //This loop adds the item to the available slot
        foreach (ItemSlot slot in slots)
        {
            if (slot.slotAvailable)
                count++;

            if (slot.slotAvailable)
                Debug.Log("----------------Loop Count " + count);




            #region Item Addition without Stacking

            if (continueToAdd)
            {
             
                Debug.LogWarning("Slot Available: " + slot.slotAvailable);
                Debug.LogWarning("Slot Empty: " + slot.slotAvailable);
                if (slot.slotAvailable && !slot.slotEmpty && newInventoryItem.isStackable)
                {
                    //Here I check if the Item I am adding to inventory matches the item in the slot
                    if (slot.inventoryItem.SO_Item == newInventoryItem.SO_Item)
                    {
                        //If it does, I check if the slot has a capcity to add that item in itself
                        if (!slot.GetStackMaxed())
                        {
                            //We stack the item in the slot
                            slot.StackItem();
                            Destroy(newInventoryItem.gameObject);
                            break;
                        }
                        else
                        {

                            //Else we continue with the loop
                            Debug.LogError("Stacked to the full");
                            continue;
                        }
                       
                    }
                }
                else if (slot.slotAvailable && slot.slotEmpty)
                {
                    slot.InitItemInSlot(newInventoryItem);
                    // Add the new item to the inventory list
                    items.Add(newInventoryItem);
                    //Add Scriptable Items to list
                    SO_Items.Add(newInventoryItem.SO_Item);
                    break;
                }
                else if (slot.slotAvailable && !slot.slotEmpty && !newInventoryItem.isStackable)
                {
                    Debug.LogWarning("Look for new slots");
                    continue;
                }
                else
                {
                    Debug.Log("Destroying Item");
                    Destroy(newInventoryItem.gameObject);

                    return;
                }
            }
            #endregion


        }

        Destroy(itemToAdd.gameObject);

    }
    #endregion


  
    /// <summary>
    /// This function is called when trying to drag and drop items in the slots
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    public void TryPlacingItem(ItemInventory item, Slot slot)
    {
        if (slot.slotAvailable && slot.slotEmpty)
        {
            if (slot.stackCount > 1)
            {
                if (slot is ItemSlot)
                {
                    ItemSlot itemSlot = (ItemSlot)slot;
                    itemSlot.ShowItemCount(slot.stackCount);
                }
            }
            slot.InitItemInSlot(item);
            item.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            Debug.Log("Slot Not Empty");
        }
    }

    #region UI
    public void OpenInventory()
    {
        if (canOpenClose)
        {

            canOpenClose = false;
            overlay.DOFade(120f / 255f, 0.3f);
            overlay.raycastTarget = true;
            model.input.ChangeState(InputController.State.Pause);
            inventoryPanel.DOScale(1f, 0.3f).OnComplete
                (
                () => canOpenClose = true
                );
        }
    }

    public void CloseInventory()
    {
        if (canOpenClose)
        {

            canOpenClose = false;
            model.input.ChangeState(InputController.State.CharacterControl);
            overlay.DOFade(0f, 0.3f).OnComplete
                (
                 () => overlay.raycastTarget = false
                );


            inventoryPanel.DOScale(0f, 0.3f).OnComplete
                (
                () => canOpenClose = true
                );
        }
    }

    #endregion

    /// <summary>
    /// Create inventory slots and add them to slot list
    /// </summary>
    public void SetupInventory()
    {
        for (int i = 0; i < totalItemSlots; i++)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, itemContainer);
            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            slot.name = "ItemSlot " + i;
            slot.gameObject.SetActive(false);
            if (i <= availbleItemSlots - 1)
            {
                slot.slotAvailable = true;
                slot.slotEmpty = true;
                slot.gameObject.SetActive(true);
            }
            slots.Add(slot);
        }
    }


   

 
    //Get Rect Transform of Item Container (Parent Gameobject)
    public RectTransform GetItemContainer()
    {
        return itemContainer.GetComponent<RectTransform>();
    }
}

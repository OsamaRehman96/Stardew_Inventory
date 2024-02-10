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
        //EventManager.TryAddingToInventory = TryAddingItemToInventory;
        EventManager.TryAddingToInventory = TryAddingItem;
        itemDictionary = new Dictionary<ScriptableItem, ItemInventory>();
    }

    private void Start()
    {
        //Initialize Inventory
        SetupInventory();
    }


    #region Inventory MAIN



    //Revamped function
    /// <summary>
    /// This function adds items to inventory and also handles stacking of items
    /// </summary>
    /// <param name="itemToAdd">Item to be added</param>
    private void TryAddingItem(Item itemToAdd)
    {
        //check if slot count >0
        if (slots.Count <= 0)
            return;


        #region Inventory Full Check
        //checking if inventory is full
        if (IsInventoryFull())
        {
            ///Check if the item to add is a stackable item, if it is continue;
            ///If it is not a stackable item then return that the inventory is full
            if (!itemToAdd.SO_Item.isStackable)
            {
                Debug.Log("Inventory is full");
                return;
            }
        }
        #endregion

        #region Instantiate ItemToAdd

        //Instantiate a new item prefab
        ItemInventory newInventoryItem = Instantiate(inventoryItemPrefab, itemContainer);
        //Assign its scriptable object type
        newInventoryItem.SO_Item = itemToAdd.SO_Item;
        //Setup inventoryItem
        newInventoryItem.SetupInventoryItem();

        #endregion

        //Lets check if item already exists in the list and what is its stack count
        #region Loop Checking

        //cache the first available slot
        ItemSlot firstEmptySlot = null;
        //bool to check if the item has been stacked or not
        bool itemStacked = false;


        foreach (ItemSlot slot in slots)
        {

#if UNITY_EDITOR
            Debug.LogWarning("Slot Available: " + slot.slotAvailable);
            Debug.LogWarning("Slot Empty: " + slot.slotEmpty);
#endif

            //keep checking for the first available slot and store it
            if (slot.slotAvailable && firstEmptySlot == null)
            {
                if (slot.slotEmpty)
                {
                    firstEmptySlot = slot;
                }

                if (items.Count == 0)
                {
                    break;
                }
            }


            if (!slot.slotEmpty && slot.slotAvailable)
            {
                Debug.Log(slot.name);
                if (slot.inventoryItem.SO_Item == newInventoryItem.SO_Item && newInventoryItem.isStackable)
                {
                    if (!slot.GetStackMaxed())
                    {
                        slot.StackItem();
                        Destroy(newInventoryItem.gameObject);
                        itemStacked = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

            }


        }

        if (!itemStacked)
        {
            if (IsInventoryFull())
            {
                Debug.Log("Item inventory us full destroying gameobject");
                Destroy(newInventoryItem.gameObject);
                return;
            }
            else
            {
                Debug.Log("adding to empty slot");

                firstEmptySlot.InitItemInSlot(newInventoryItem);
                items.Add(newInventoryItem);
                SO_Items.Add(newInventoryItem.SO_Item);
            }
        }


        Destroy(itemToAdd.gameObject);

        #endregion
    }


    /// <summary>
    /// Function to check if inventory is full
    /// </summary>
    /// <returns>returns true if inventory is full</returns>
    private bool IsInventoryFull()
    {
        if (availbleItemSlots == items.Count)
        {
            return true;
        }
        else
            return false;
    }



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

    #endregion

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


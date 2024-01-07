using UnityEngine;
using System;

public static class EventManager
{
    public static Action<Transform> ShowInteractCanvas;
    public static Action HideInteractCanvas;


    //Inventory
    public static Action<Item> TryAddingToInventory;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Item class is used to store the values of items and display it in the inventory system
public class Item : MonoBehaviour
{
    //Scriptable Object that contains items data
    public ScriptableItem SO_Item;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Sets up sprites from the SO to the item
    public void SetupSprite()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = SO_Item.visualSprite;

    }

  
}

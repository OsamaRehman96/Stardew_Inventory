using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGM.Gameplay;

//This class handles the behavior of pickable equipment
public class EquippableItem : Item,I_Interactable
{

    private CharacterController2D controller;

    // Start is called before the first frame update
    void Start()
    {
        SetupItem();
    }

    public void SetupItem()
    {
        SetupSprite();
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller = collision.gameObject.GetComponent<CharacterController2D>();
            controller.interactableObject = this;
            ShowInteractable();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller != null)
            {
                controller.interactableObject = null;
                controller = null;
            }
            HideInteractable();
        }
    }

    public void ShowInteractable()
    {
        EventManager.ShowInteractCanvas(transform);
    }

    public void HideInteractable()
    {
        EventManager.HideInteractCanvas();
    }

    public void Interact()
    {
        Debug.Log("Trying to pick up the " + SO_Item.name + " item");
        EventManager.TryAddingToInventory(gameObject.GetComponent<Item>());
    }
}

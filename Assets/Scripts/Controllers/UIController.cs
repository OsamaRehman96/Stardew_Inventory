using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject interactableCanvas;
    [SerializeField] private Inventory inventory;

    [SerializeField] private ItemDisplay itemDisplay;

    private bool isMenuOpen = false;
    
    

    private void Awake()
    {
        EventManager.ShowInteractCanvas = ShowInteractable;
        EventManager.HideInteractCanvas = HideInteractable;
    }

    private void ShowInteractable(Transform _transform)
    {
        Vector2 pos = _transform.localPosition;
        pos.y += .8f;
        interactableCanvas.transform.localPosition = pos;
        interactableCanvas.SetActive(true);
    }

    private void HideInteractable()
    {
        interactableCanvas.SetActive(false);
    }

    public void OpenCloseMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = false;
            inventory.CloseInventory();
        }
        else
        {
            isMenuOpen = true;
            inventory.OpenInventory();
        }

    }
}

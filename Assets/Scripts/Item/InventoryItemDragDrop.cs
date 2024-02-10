using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler,IPointerExitHandler
{

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private ItemInventory item;
    private ItemSlot currentSlot;

    private DropItemArea dropArea;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        item = GetComponent<ItemInventory>();
        //currentSlot = GetComponent
    }

   

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;
        DragDropSystem.instance.StartedDragging(item);
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        


        DragDropSystem.instance.StoppedDragging();
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError(item.name);
        
        //throw new System.NotImplementedException();k
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemDisplay.ShowItemDisplay(item.name, item.transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDisplay.HideDisplay();
    }




}

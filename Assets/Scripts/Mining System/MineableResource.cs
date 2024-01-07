using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGM.Gameplay;
using DG.Tweening;
public class MineableResource : MonoBehaviour, I_Interactable
{

    private CharacterController2D controller;
    [SerializeField] Transform visual;

    /// <summary>
    /// can the resource be mined
    /// </summary>
    bool canHarvest = true;

    /// <summary>
    /// is resource empty
    /// </summary>
    public bool isEmpty { get; set; }

    public SpriteRenderer GetSpriteRenderer()
    {
        return visual.GetComponent<SpriteRenderer>();
    }

    public void ShowInteractable()
    {
        EventManager.ShowInteractCanvas(this.transform);
    }

    public void HideInteractable()
    {
        EventManager.HideInteractCanvas();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEmpty)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            controller = collision.gameObject.GetComponent<CharacterController2D>();
            controller.interactableObject = this;

            ShowInteractable();
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isEmpty)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (controller != null)
            {
                controller.interactableObject = null;
                controller = null;
            }

            HideInteractable();
        }
    }

    public void Interact()
    {
        if (canHarvest && !isEmpty)
        {
            canHarvest = false;
            Shake();

            OnInteract();
        }
    }

    private void Shake()
    {
        float duration = 0.5f;
        Vector3 strength = new Vector3(0.2f, 0.2f, 0);

        visual.DOPunchScale(strength, duration).OnComplete
            (
             () => canHarvest = true
            );
    }

    public virtual void OnInteract()
    {
        //Do something
    }
}

using RPGM.Core;
using RPGM.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class DropItemArea : MonoBehaviour, I_Dropable
{

    GameModel model = Schedule.GetModel<GameModel>();

    

    public void OnItemDropped(ItemInventory item)
    {

        Debug.Log("Dropping Item " + item.name);
        SpawnAndSpringPrefab(item);
    }

    private void SpawnAndSpringPrefab(ItemInventory item)
    {

        // Define the radius of the circle and the minimum distance from the center
        float circleRadius = 1f; // Change this value to your desired radius
        float minDistanceFromCenter = 0.2f; // Change this value to your desired minimum distance

        // Calculate a random angle within a circle
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate a random distance within the circle radius (while ensuring it is at least minDistanceFromCenter away from the center)
        float randomDistance = Random.Range(minDistanceFromCenter, circleRadius);

        // Calculate spawn position based on random angle and distance
        Vector3 spawnPosition = model.player.transform.position + new Vector3(randomDistance * Mathf.Cos(randomAngle), 0f, randomDistance * Mathf.Sin(randomAngle));

        // Instantiate the prefab at the specified spawn position

        bool isEquipment = (item.SO_Item.itemType == ItemType.equipment) ? true : false;

        GameObject newPrefab = new GameObject();
        newPrefab.name = item.name + "(Pickable)";
        newPrefab.transform.position = spawnPosition;
        newPrefab.transform.rotation = Quaternion.identity;
            //Instantiate(new GameObject(), spawnPosition, Quaternion.identity);

        if (isEquipment)
        {
            newPrefab.AddComponent<SpriteRenderer>();
            EquippableItem equipableItem = newPrefab.AddComponent<EquippableItem>();
            equipableItem.SO_Item = item.SO_Item;
            equipableItem.SetupItem();
        }
        else
        {
           
            newPrefab.AddComponent<SpriteRenderer>();
            PickableItem pickableItem = newPrefab.AddComponent<PickableItem>();

            pickableItem.SO_Item = item.SO_Item;
            //Setup Item
            pickableItem.SetupItem();

        }


        // Apply the spring animation using DoTween
        newPrefab.transform.localScale = Vector3.zero; // Start with scale 0 to make it appear from nothing
        newPrefab.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce); // Spring animation from scale 0 to 1

        // Apply the spring position animation
        Vector3 originalPosition = newPrefab.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * 0.5f; // Adjust the spring height
        newPrefab.transform.DOMove(targetPosition, 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
        {

            // Debug.Log("Spring animation complete!");
        });


    }

}

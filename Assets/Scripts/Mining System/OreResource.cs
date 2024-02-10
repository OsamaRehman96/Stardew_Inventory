using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OreResource : MineableResource
{
    /// <summary>
    /// How many hits it takes to break the object
    /// </summary>
    public float hitsToBreak = 5;

    private int hitCount = 0;

    [SerializeField] private GameObject hitEffectPrefab;

    [SerializeField] private GameObject itemToInstantiate;

    /// <summary>
    /// Can this resource give the player multiple items on breaking
    /// </summary>
    public bool canSpawnMutliple;

    public override void OnInteract()
    {
        hitCount++;

        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, this.transform);
            effect.transform.localPosition = Vector3.zero;

        }

        if (hitCount == hitsToBreak)
        {
            Debug.Log("Harvested Ore");
            // Empty the resource
            isEmpty = true;
            // Hide the interactable pop up
            HideInteractable();
            GetSpriteRenderer().material.DOFade(0.2f, 0.3f);
            if (canSpawnMutliple)
                for (int i = 0; i < 4; i++)
                {
                    SpawnAndSpringPrefab();
                }
            else
                SpawnAndSpringPrefab();
                
        }

        Debug.Log("This function is called from the Ore Resource");
    }


    private void SpawnAndSpringPrefab()
    {

        // Define the radius of the circle and the minimum distance from the center
        float circleRadius = 1f; // Change this value to your desired radius
        float minDistanceFromCenter = 0.2f; // Change this value to your desired minimum distance

        // Calculate a random angle within a circle
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate a random distance within the circle radius (while ensuring it is at least minDistanceFromCenter away from the center)
        float randomDistance = Random.Range(minDistanceFromCenter, circleRadius);

        // Calculate spawn position based on random angle and distance
        Vector3 spawnPosition = transform.position + new Vector3(randomDistance * Mathf.Cos(randomAngle), 0f, randomDistance * Mathf.Sin(randomAngle));

        // Instantiate the prefab at the specified spawn position
        GameObject newPrefab = Instantiate(itemToInstantiate, spawnPosition, Quaternion.identity);
        PickableItem pickableItem = newPrefab.GetComponent<PickableItem>();

        //Setup Item
        pickableItem.SetupItem();


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

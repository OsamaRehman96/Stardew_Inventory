using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This class handles the display of each item when you hover of it in inventory
public class ItemDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private GameObject hintPanel;

    public static System.Action<string,Vector3> ShowItemDisplay;
    public static System.Action HideDisplay;

    public Vector3 offset;

    private void Awake()
    {
        ShowItemDisplay = ShowDisplay;
        HideDisplay = Hide;
    }

    private void ShowDisplay(string name,Vector3 position)
    {
        hintPanel.SetActive(true);
        itemName.text = name;
        hintPanel.transform.position = position + offset;
    }

    private void Hide()
    {
        hintPanel.SetActive(false);
    }
}

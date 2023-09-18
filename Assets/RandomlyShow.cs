using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyShow : MonoBehaviour
{
    private List<Transform> childList = new List<Transform>();
    private int buttonPressCount = 0;

    public GameObject button;

    private void Start()
    {
        // Get all children of the object and add them to the list.
        foreach (Transform child in transform)
        {
            childList.Add(child);
        }
    }

    public void ActivateRandomChild()
    {
        // Check if there are any children left to activate.
        if (childList.Count > 0)
        {
            int randomIndex = Random.Range(0, childList.Count);
            Transform randomChild = childList[randomIndex];

            // Activate the random child and remove it from the list.
            randomChild.gameObject.SetActive(true);
            childList.RemoveAt(randomIndex);

            buttonPressCount++;

            // Check if all children have been activated.
            if (childList.Count == 0)
            {
                button.SetActive(false);
            }
        }
    }
}

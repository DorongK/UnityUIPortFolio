using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public Item itemdata;

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", itemdata.itemName);
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }
}

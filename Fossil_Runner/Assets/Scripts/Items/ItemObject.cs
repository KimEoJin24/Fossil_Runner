using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPrompt()
    {
        return string.Format("선택 {0}", item.displayName);
    }

    public void OnInteract()
    {
        Inventory.instance.AddItem(item); // Inventory에서
        Destroy(gameObject);
    }
}

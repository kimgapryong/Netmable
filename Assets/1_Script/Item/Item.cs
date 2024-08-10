using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    private InventoryManager inventoryManager;

    public string itemName;
    public Sprite itemIcon;
    public int count;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }
    public abstract void UseItem();
        
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
   public void Open()
    {
        UiManager.Instance.inventory.SetActive(true);
        UiManager.Instance.inventoryButton.gameObject.SetActive(false);
    }
    public void Close()
    {
        UiManager.Instance.inventory.SetActive(false);
        UiManager.Instance.inventoryButton.gameObject.SetActive(true);
    }
}

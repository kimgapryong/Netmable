using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public AudioClip openClip;
    public AudioClip closeClip;
   public void Open()
    {
        SoundManager.Instance.SFXSound("Open", openClip);
        UiManager.Instance.inventory.SetActive(true);
        UiManager.Instance.inventoryButton.gameObject.SetActive(false);
    }
    public void Close()
    {
        SoundManager.Instance.SFXSound("Close", closeClip);
        UiManager.Instance.inventory.SetActive(false);
        UiManager.Instance.inventoryButton.gameObject.SetActive(true);
    }
}

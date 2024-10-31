using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject slotPrefab;
    public Transform invenPanel;
    public GameObject selectedItem;
    private Dictionary<string, ItemData> itemSlots = new Dictionary<string, ItemData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (itemSlots.ContainsKey(itemData.itemName))
        {
            itemSlots[itemData.itemName].IncreaseCount(); // count 증가
            UpdateInventoryUI(itemSlots[itemData.itemName]);
        }
        else
        {
            itemSlots[itemData.itemName] = itemData;

            GameObject newSlot = Instantiate(slotPrefab, invenPanel);
            Image itemImage = newSlot.transform.Find("ItemButton/Item").GetComponent<Image>();
            itemImage.sprite = itemData.itemIcon;

            itemData.IncreaseCount();
            newSlot.GetComponentInChildren<Text>().text = $"{itemData.GetCount()}";  // 실시간으로 count 값 참조
            newSlot.name = itemData.itemName;

            GameObject useButton = newSlot.transform.Find("UseButton").gameObject;
            GameObject cancelButton = newSlot.transform.Find("NotButton").gameObject;

            useButton.SetActive(false);
            cancelButton.SetActive(false);

            useButton.GetComponent<Button>().onClick.AddListener(() => UseSelectedItem(itemData, useButton, cancelButton));
            cancelButton.GetComponent<Button>().onClick.AddListener(() => CancelItemSelection(useButton, cancelButton));

            Button itemButton = newSlot.transform.Find("ItemButton").GetComponent<Button>();
            itemButton.onClick.AddListener(() => OnItemClick(itemData, newSlot));
        }
    }

    private void OnItemClick(ItemData itemData, GameObject slot)
    {
        GameObject useButton = slot.transform.Find("UseButton").gameObject;
        GameObject cancelButton = slot.transform.Find("NotButton").gameObject;

        selectedItem = slot;
        useButton.SetActive(true);
        cancelButton.SetActive(true);
    }

    private void UseSelectedItem(ItemData itemData, GameObject useButton, GameObject cancelButton)
    {
        if (itemData != null)
        {
            itemData.UseItem();
            UpdateInventoryUI(itemData);
            useButton.SetActive(false);
            cancelButton.SetActive(false);
        }
    }

    private void CancelItemSelection(GameObject useButton, GameObject cancelButton)
    {
        useButton.SetActive(false);
        cancelButton.SetActive(false);
    }

    private void UpdateInventoryUI(ItemData itemData)
    {
        if (itemData.GetCount() <= 0)
        {
            GameObject slotToRemove = invenPanel.Find(itemData.itemName)?.gameObject;
            if (slotToRemove != null)
            {
                Destroy(slotToRemove);
                itemSlots.Remove(itemData.itemName);
            }
        }
        else
        {
            GameObject slotToUpdate = invenPanel.Find(itemData.itemName)?.gameObject;
            if (slotToUpdate != null)
            {
                slotToUpdate.GetComponentInChildren<Text>().text = $"{itemData.GetCount()}";  // 실시간으로 count 값 업데이트
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance { get; private set; }

    public GameObject slotPregab;
    public Transform invenPanel;
    public GameObject selectedItem; // 현재 선택된 아이템



    private Dictionary<string, GameObject> itemSlots = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(Item item)
    {
        if(itemSlots.ContainsKey(item.itemName))
        {
            item.count++;
            itemSlots[item.itemName].GetComponentInChildren<Text>().text = $"{item.count}";
        }
        else
        {
            GameObject newSlot = Instantiate(slotPregab, invenPanel);
            Image itemImage = newSlot.GetComponentInChildren<Image>();
            newSlot.GetComponentInChildren<Image>().sprite = item.itemIcon; 
            newSlot.GetComponentInChildren<Text>().text = $"{item.count}";

            itemSlots[item.itemName] = newSlot;

            EventTrigger trigger = itemImage.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnItemClick(item, newSlot); });
            trigger.triggers.Add(entry);

            GameObject useButton = newSlot.transform.Find("UseButton").gameObject;
            GameObject cancelButton = newSlot.transform.Find("NotButton").gameObject;

            useButton.SetActive(false); 
            cancelButton.SetActive(false); 

          
            useButton.GetComponent<Button>().onClick.AddListener(() => UseSelectedItem(item, useButton, cancelButton));

            cancelButton.GetComponent<Button>().onClick.AddListener(() => CancelItemSelection(useButton, cancelButton));
        }
    }

    private void OnItemClick(Item item, GameObject slot)
    {
       
        GameObject useButton = slot.transform.Find("UseButton").gameObject;
        GameObject cancelButton = slot.transform.Find("NotButton").gameObject;
       
        selectedItem = slot;
        useButton.SetActive(true);
        cancelButton.SetActive(true);
    }

    private void UseSelectedItem(Item item, GameObject useButton, GameObject cancelButton)
    {
        if (item != null)
        {
            item.UseItem(); // 아이템 사용
            UpdateInventoryUI(item); // UI 업데이트
            useButton.SetActive(false); // 사용 후 버튼 비활성화
            cancelButton.SetActive(false); 
        }
    }

    private void CancelItemSelection(GameObject useButton, GameObject cancelButton)
    {
        useButton.SetActive(false); 
        cancelButton.SetActive(false);
    }
    private void UpdateInventoryUI(Item item)
    {
        
        if (item.count <= 0)
        {
            Destroy(selectedItem); 
            itemSlots.Remove(item.itemName); 
        }
        else
        {
            selectedItem.GetComponentInChildren<Text>().text = $"{item.count}";
        }
    }
}

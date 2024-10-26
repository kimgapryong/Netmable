using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private InventoryManager inventoryManager;

    public string itemName;
    public Sprite itemIcon;
    public int count;
    
    private Collider2D itemCollider;

    private void Awake()
    {
        itemCollider = GetComponent<Collider2D>();
        itemCollider.enabled = false;
        StartCoroutine(EnableColliderAfterDelay(0.5f));
        inventoryManager = InventoryManager.Instance;
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        itemCollider.enabled = true;
    }

    public ItemData GetItemData()
    {
        ItemData itemData = new ItemData(itemName, itemIcon, () => count, IncrementCount, UseItem)
        {
            UseAction = UseItem
        };
        return itemData;
    }

    public void IncrementCount()
    {
        count++;
    }

    public virtual void UseItem()
    {
        Debug.Log("기본 아이템 사용");
    }
}
[Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemIcon;
    public Func<int> count;
    public Action IncrementCount;
    public Action UseAction;

    public ItemData(string name, Sprite sprite,Func<int> itemCount, Action incrementCount, Action UseItem)
    {
        itemName = name;
        itemIcon = sprite;
        count = itemCount;
        IncrementCount = incrementCount;
        UseAction = UseItem;
    }

    public void UseItem()
    {
        UseAction?.Invoke();
    }

    public int GetCount()
    {
        return count();  // 실시간으로 count 값을 가져옴
    }

    public void IncreaseCount()
    {
        IncrementCount?.Invoke();
    }
}
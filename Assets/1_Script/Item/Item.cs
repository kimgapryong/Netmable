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
        ItemData itemData = new ItemData(() => count, IncrementCount)
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
        Debug.Log("�⺻ ������ ���");
    }
}
[Serializable]
public class ItemData
{

    public Func<int> count;
    public Action IncrementCount;
    public Action UseAction;

    public ItemData(Func<int> itemCount, Action incrementCount)
    {

        count = itemCount;
        IncrementCount = incrementCount;
    }

    public void UseItem()
    {
        UseAction?.Invoke();
    }

    public int GetCount()
    {
        return count();  // �ǽð����� count ���� ������
    }

    public void IncreaseCount()
    {
        IncrementCount?.Invoke();
    }
}
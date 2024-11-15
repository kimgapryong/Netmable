using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }


    public List<GameObject> items = new List<GameObject>();
    public float dropChance = 0f;


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


    public void RandomItem(Vector3 dropPosition)
    {
        if(Random.value >= dropChance)
        {
            int randomIndex = Random.Range(0, items.Count);
            GameObject itemToDrop = items[randomIndex];
            Debug.Log("아이템");

            // itemToDrop이 유효한지 확인
            if (itemToDrop != null)
            {
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }
        
     
    }
}

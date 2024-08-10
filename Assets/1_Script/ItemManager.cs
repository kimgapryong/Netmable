using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }


    public List<GameObject> items;
    public float dropChance = 0.9f;


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


    private void Start()
    {

        items = new List<GameObject>();
        Item[] foundItems = FindObjectsOfType<Item>();

        foreach (Item item in foundItems)
        {
            items.Add(item.gameObject);
            Debug.Log("Added item: " + item.name);
        }
    }


    public void RandomItem(Vector3 dropPosition)
    {

        if (Random.value >= dropChance)
        {
            int randomIndex = Random.Range(0, items.Count);
            GameObject itemToDrop = items[randomIndex];
            Instantiate(itemToDrop, dropPosition, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public int count;
   
    public virtual void UseItem()
    {
        
    }
}

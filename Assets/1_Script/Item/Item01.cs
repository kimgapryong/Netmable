using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item01 : Item
{

    public int itemNum = 5;
    public override void UseItem()
    {
        Debug.Log("tkdyd");
        if(count >= itemNum)
        {
            PlayerManager.Instance.playerStatus.damage += 5;
            PlayerManager.Instance.playerStatus.maxHp += 20;
            PlayerManager.Instance.playerStatus.maxMp += 10;
            PlayerManager.Instance.playerStatus.speed += 1;

            count -= itemNum;
        }
    }

  
    private void Start()
    {
        itemName = "운석조각";
    }


}

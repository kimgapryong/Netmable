using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : Item
{

    private int amount;
    public int addHp = 50;
    public override void UseItem()
    {
        if (count > 0)
        {
            if(status.maxHp - status.currentHp < addHp)
            {
                amount = status.maxHp - status.currentHp;
                status.currentHp += amount;
            }
            else
            {
                status.currentHp += addHp;
            }
            count--;
        }
    }


    private void Start()
    {
        status = PlayerManager.Instance.playerStatus;
        itemName = "ü������";
    }

}
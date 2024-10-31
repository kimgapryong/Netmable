using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaItem : Item
{

    private int addMana = 30;

    private void Start()
    {
        status = PlayerManager.Instance.playerStatus;
        itemName = "마나물약";
    }
    public override void UseItem()
    {
        if((status.maxMp - status.currentMp) < addMana)
        {
            status.currentMp += status.maxMp - status.currentMp;
           
        }
        else
        {
            status.currentMp += addMana;
        }
        count--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState 
{
    protected Boss boss;

    public void SetBoss(Boss boss)
    {
        this.boss = boss;
        Debug.Log(boss.name);
    }

    protected abstract void PlaLogic();
}

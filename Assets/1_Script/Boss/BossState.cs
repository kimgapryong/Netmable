using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{

    public delegate void BossStateEvent();
    public event BossStateEvent bossEvent;

    protected Boss boss;
    public BossState(Boss boss)
    {
        this.boss = boss;
    }

    public void SetBoss(Boss boss)
    {
        this.boss = boss;
        Debug.Log(boss.name);
    }

    public abstract void OnstateEnter();
    public abstract void OnstateUpdate();
    public abstract void OnstateExit();

}

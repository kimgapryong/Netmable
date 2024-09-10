using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour 
{
    protected Boss boss;

    public void SetBoss(Boss boss)
    {
        this.boss = boss;
    }

    protected abstract void PlaLogic();
}

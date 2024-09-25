using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossState
{
    public IdleState(Boss boss) : base(boss) { }
    public override void OnstateEnter()
    {
       //animation
    }

    public override void OnstateExit()
    {
        boss.BossIdle();
    }

    public override void OnstateUpdate()
    {
        //animation
    }
}

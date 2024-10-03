using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BossState
{
    public MoveState(Boss boss) : base(boss)
    {
    }

    public override void OnstateEnter()
    {
       boss.find.enabled = true;
    } 

    public override void OnstateExit()
    {
        
    }

    public override void OnstateUpdate()
    {
        boss.find.enabled = false;
    }
}

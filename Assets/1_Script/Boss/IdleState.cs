using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossState
{
    public IdleState(Boss boss) : base(boss)
    {
    }
    
    public override void OnstateEnter()
    {
      
        //animation
        boss.isIdle = false;
        boss.BossIdle();
    }

    public override void OnstateExit()
    {
        
    }

    public override void OnstateUpdate()
    {
      
        boss.transform.position = boss.fixedPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private Boss.State bossState;
    private Dictionary<Boss.State, BossState> stateDictionary;
    public FSM(Boss.State state)
    {
        stateDictionary = new Dictionary<Boss.State, BossState> ();
        bossState = state;
    }

    public void ChangeState(Boss.State nextState)
    {
        if(bossState == nextState) return;

        if (bossState != null) bossState.OnstateExit();
        bossState = nextState;
        bossState.OnstateEnter();
    }
    
    public void UpdateState()
    {
        if(bossState != null) bossState.OnstateUpdate();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private Boss.State bossState;  
    public Dictionary<Boss.State, BossState> stateDictionary; 


    public FSM(Boss.State initialState, Dictionary<Boss.State, BossState> stateDict)
    {
        bossState = initialState;
        stateDictionary = stateDict;
    }


    public void ChangeState(Boss.State nextState)
    {
        if (bossState == nextState) return;  

        stateDictionary[bossState].OnstateExit(); 

        bossState = nextState;

        stateDictionary[bossState].OnstateEnter();  
    }


    public void UpdateState()
    {
        if (stateDictionary[bossState] != null)
            stateDictionary[bossState].OnstateUpdate();
    }
}

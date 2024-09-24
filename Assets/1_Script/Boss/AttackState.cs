using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BossState
{
    private bool bossAttack = true;
    private float coolTime = 5f;
    protected override void PlaLogic()
    {
        if (bossAttack)
        {

        }
    }
    private IEnumerator BossAttackCool()
    {
        bossAttack = false;
        yield return new WaitForSeconds(coolTime);
        bossAttack = true;
    }

}

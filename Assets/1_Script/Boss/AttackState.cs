using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : BossState
{

    private Rigidbody2D rigidbody2;

    public AttackState(Boss boss) : base(boss) { }


    public override void OnstateEnter()
    {
        rigidbody2 = boss.GetComponent<Rigidbody2D>();
        rigidbody2.velocity = new Vector2(0, rigidbody2.velocity.y);
        boss.isAttack = false;
    }

    public override void OnstateExit()
    {
        boss.isAttack = true;
    }

    public override void OnstateUpdate()
    {
        int randomAttack = Random.Range(0, boss.skilCount);
        boss.bossSkils[randomAttack]?.Invoke();
    }



}

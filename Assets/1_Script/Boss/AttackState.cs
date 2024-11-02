
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : BossState
{

    private Rigidbody2D rigidbody2;
    private int attackCount = 0;
    public AttackState(Boss boss) : base(boss) { }


    public override void OnstateEnter()
    {
        
        rigidbody2 = boss.GetComponent<Rigidbody2D>();
        rigidbody2.velocity = new Vector2(0, rigidbody2.velocity.y);
        boss.isAttack = false;
    }

    public override void OnstateExit()
    {
       
    }

    public override void OnstateUpdate()
    {
      
        if (!boss.attackCool)
        {
         
            if(attackCount <= 3)
            {
                attackCount++;
                int randomAttack = Random.Range(0, boss.skilCount - 1);
                boss.StartCoroutine(boss.bossSkils[randomAttack]?.Invoke());
            }
            else
            {
                attackCount = 0;
                boss.find.enabled = false;
                boss.StartCoroutine(boss.Attack2());
            }
           

        }
      
    }



}

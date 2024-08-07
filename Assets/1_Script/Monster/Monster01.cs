using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01 : Monster
{
    [SerializeField] private MonsterData _data;
    private Transform monsterGround;
    public LayerMask ground;

    private void Start()
    {
        ResetData(_data);
        monsterGround = transform.Find("GroundCheck");
    }
    protected override void MonsterMove()
    {
        bool isGround = Physics2D.OverlapCapsule(monsterGround.position, new Vector2(4f, 1f), CapsuleDirection2D.Horizontal, 0, ground);
        if (isAttack && isGround)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    protected override void MonsterSkils()
    {
        throw new System.NotImplementedException();
    }


}

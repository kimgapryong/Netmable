using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster04 : Monster
{
    public MonsterData data;
    public float monDis = 40f;
    public float monAtt = 20f;
    private Vector2 length;

    private void Start()
    {
        ResetData(data);
    }
    protected override void Update()
    {
        MonsterMove();
        Check();
    }
    public override void Check()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            if (player.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
            }
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y));
            }
        }
    }
    protected override void MonsterMove()
    {
        length = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(length.y, length.x) * Mathf.Rad2Deg);
    }

    protected override void MonsterSkils()
    {
        //throw new System.NotImplementedException();
    }

   
}

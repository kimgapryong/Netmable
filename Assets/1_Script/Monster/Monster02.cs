using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster02 : Monster
{
    [SerializeField]
    private MonsterData data;

    public GameObject obj;
    private GameObject objW;
    public Transform groundCheck;
    public Transform attackPos;

    private Rigidbody2D rigid;
    public LayerMask ground;

    private bool coolTime = true;
    private bool isGround;
    public bool isStop = true;

    private void Start()
    {
        ResetData(data);
        rigid = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void MonsterMove()
    {
        isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(4f, 1f), CapsuleDirection2D.Horizontal, 0, ground);
        if (isAttack && isGround && player != null && isStop)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    protected override void MonsterSkils()
    {
        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 21 && coolTime)
            {
                coolTime = false;
                if (objW == null)
                {
                    objW = Instantiate(obj, attackPos);
                    objW.transform.position = attackPos.position;
                    objW.GetComponent<Monster02_skilObj>().monster = attackPos;
                    objW.GetComponent<Monster02_skilObj>().monsters = this;
                    objW.GetComponent<Monster02_skilObj>().damage = damage;
                }
                
                StartCoroutine(skillCool(objW));
            }
        }
    }

    private IEnumerator skillCool(GameObject clone)
    {
        
        while(true)
        {
            if(clone == null)
            {
                yield return new WaitForSeconds(3);
                coolTime = true;
                yield break;
            }
            yield return null;
        }
    }
}

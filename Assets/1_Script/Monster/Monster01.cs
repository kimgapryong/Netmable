using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01 : Monster
{
    [SerializeField] private MonsterData _data;

    public Animator animator;

    private Rigidbody2D rigid;
    private TrailRenderer trail;
    private Transform monsterGround;
    public LayerMask ground;

    

    private bool isGround;

    private bool dash = true;
    private bool isDash;
    private float dashPower = 19f;
    private float dashTime = 0.4f;
    private float dashCoolTime = 5f;

    private void Start()
    {
        ResetData(_data);
        monsterGround = transform.Find("GroundCheck");
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();

        
    }
    protected override void Update()
    {
        base.Update();

        ScaleCheck();
       
        
    }
    protected override void MonsterMove()
    {

        isGround = Physics2D.OverlapCapsule(monsterGround.position, new Vector2(4f, 1f), CapsuleDirection2D.Horizontal, 0, ground);
        if (isAttack && isGround && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void ScaleCheck()
    {
   
        if (Vector2.Distance(transform.position, player.transform.position) <= 5f)
        {
            animator.SetBool("isAttack", true);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }
    protected override void MonsterSkils()
    {
        Debug.Log("3");
        if (player != null)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 14 && dash)
            {

                StartCoroutine(Dash());
            }
        }
        
    }
    private IEnumerator Dash()
    {
        //if(trail == null) { yield break; }

        dash = false;
        isGround = false;
        isDash = true;
        float orign = rigid.gravityScale;
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(-transform.localScale.x * dashPower, 0f);
        if(trail != null)
        {
            trail.emitting = true;
        }
       
        yield return new WaitForSeconds(dashTime);
        
        if(trail != null)
        {
            trail.emitting = false;
        }
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = orign;
        isDash = false;
        isGround = true;
        yield return new WaitForSeconds(dashCoolTime);
        dash = true;
    }

}

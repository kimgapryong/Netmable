using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03 : Monster
{
    public MonsterData data;
    public Transform groundCheck;
    public Transform rayCheck;
    public LayerMask ground;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 transMove;
    private Vector2 weightGravity;

    public float jumpForce = 30f;
    public float downPower = 0.5f;
    public float plaDistance = 20f;
    public float coolTime = 4f;

    public bool groundOne = false;
    public bool canJump = false;
    public bool attackJump = true;
    public bool canMove = true;
    public bool isGround;
    public bool monsterUp = true;

    public bool isRay;
    
    protected override void Update()
    {
        base.Update();
        isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(4f, 1f), CapsuleDirection2D.Horizontal, 0, ground);

        //레이케스트 확인
        RaycastHit2D hit = Physics2D.Raycast(rayCheck.position, rightCheck, 4f, ground);
        isRay = hit.collider != null;
        Debug.DrawRay(transform.position, rightCheck * 4f, Color.red);
        if (Vector2.Distance(player.transform.position, transform.position) <= plaDistance)
        {
            canJump = true;
        }
        else
        {
            
            canJump= false;
        }

        if (isGround)
        {
            if(groundOne)
            {
                groundOne = false;
                StartCoroutine(coolControal());
            }
            
        }
    }
    private void Start()
    {
        ResetData(data);
        rb = GetComponent<Rigidbody2D>();
        weightGravity =new Vector2(0, -Physics2D.gravity.y);
        animator = GetComponent<Animator>();
    }
   
    protected override void MonsterMove()
    {
        if(canJump)
        {
            if(!isRay)
            {
                
                if (attackJump)
                {
                    SoundManager.Instance.BossSound("monster3", monClip);
                    attackJump = false;
                    animator.SetBool("isAtk", true);
                    rb.AddForce(new Vector2(0, jumpForce));
                    rb.velocity =new Vector2(0, jumpForce);
                    groundOne = true;
                    transMove = new Vector3(transform.localScale.x, 0);
                }
                if (canMove)
                {
                    transform.Translate(transMove * speed * Time.deltaTime);
                   
                    //rb.velocity = transMove * speed;
                }
            }
        }
    }

    protected override void MonsterSkils()
    {
 
        if (rb.velocity.y < 0)
        {
            canMove = false;
            rb.velocity -= weightGravity * downPower * Time.deltaTime;
            animator.SetBool("isAtk", false);
        }
        
        if(health <= MaxHp / 2)
        {
            if(monsterUp)
            {
                monsterUp = false;
                damage = 200;
                speed = 40f;
                jumpForce -= 5;
                weightGravity *= 2;
            }
        }
    }

    private IEnumerator coolControal()
    {
        yield return new WaitForSeconds(coolTime);
        canMove = true;
        attackJump = true;
        canJump = true;
    }
}

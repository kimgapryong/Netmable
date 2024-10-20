using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03 : Monster
{
    public MonsterData data;
    public Transform groundCheck;
    public LayerMask ground;

    private Vector2 weightGravity;
    private Rigidbody2D rb;

    public float jumpForce = 30f;
    public float downPower = 0.5f;
    public float plaDistance = 20f;
    public float coolTime = 4f;

    public bool groundOne = false;
    public bool canJump = false;
    public bool attackJump = true;
    public bool canMove = true;
    public bool isGround;
    
    protected override void Update()
    {
        base.Update();
        isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(4f, 1f), CapsuleDirection2D.Horizontal, 0, ground);
        if (Vector2.Distance(player.transform.position, transform.position) <= plaDistance)
        {
            canJump = true;
        }
        else
        {
            Debug.Log("2");
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
    }
    protected override void MonsterMove()
    {
        if(canJump)
        {
            if(attackJump)
            {
                attackJump = false;
                rb.AddForceY(jumpForce);
                rb.velocityY = jumpForce;
                groundOne = true;
            }
            if(canMove)
            {
                transform.Translate(new Vector3(transform.localScale.x, 0) * speed * Time.deltaTime);
            }
        }
    }

    protected override void MonsterSkils()
    {
        if(rb.velocity.y < 0)
        {
            canMove = false;
            rb.velocity -= weightGravity * downPower * Time.deltaTime;
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

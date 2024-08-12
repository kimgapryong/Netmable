using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;


    private float downPower = 6f;
    private float horizontal;

    private float wallJumpCooldown = 0.4f; // 벽 점프 쿨다운 시간
    private float lastWallJumpTime;

    private bool isEnemy = false;
    private bool isGround;
    private bool isWall;
    private bool wallJump;
    private bool wallSliding;
    private bool doubleJump;
    public bool facingRight = true;
    private bool wallJumped; // 벽 오른쪽 확인

    private Transform checkGround;
    private Transform checkWall;

    public LayerMask ground;
    public LayerMask wall;

    private Rigidbody2D rigid;

    private Vector2 gravityVec;

    private float originalGravityScale; // 원래 중력 값 저장

    private void Start()
    {
        animator = GetComponent<Animator>();
        checkGround = transform.Find("CheckGround");
        checkWall = transform.Find("CheckWall");

        rigid = GetComponent<Rigidbody2D>();
        gravityVec = new Vector2(0, -Physics2D.gravity.y);
        originalGravityScale = rigid.gravityScale; // 시작할 때 중력 값을 저장
    }

    private void Update()
    {
        CheckBoolType();
        StopSliding();
        PlayerMove();
        PlayerJump();
        SlidingWall();
    }
    

    private void PlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (!wallSliding)
        {
            float targetSpeed = horizontal * PlayerManager.Instance.playerStatus.speed;
            rigid.velocity = new Vector2(Mathf.Lerp(rigid.velocity.x, targetSpeed, 0.1f), rigid.velocity.y);
            Flip(horizontal);
            

            if (Mathf.Abs(horizontal) > 0)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);
            }
            else if(horizontal == 0) 
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isEnemy = true;
        }
    }
    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround || isEnemy)
            {
                isEnemy = false;
                rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
                animator.SetBool("isJump", true) ;
                animator.SetBool("isGround", false);
            }
            else if (doubleJump)
            {
                doubleJump = false;
                rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
                animator.SetBool("isJump", false);
                animator.SetBool("isDouble", true ) ;
            }
        }

        if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigid.gravityScale = originalGravityScale * 1.5f; // 상승 속도를 더 빠르게
        }
        else if (rigid.velocity.y < 0)
        {
            
            rigid.gravityScale = originalGravityScale * 2f; // 하강 속도를 더 빠르게
            rigid.velocity -= gravityVec * downPower * Time.deltaTime;
        }
        else
        {
            rigid.gravityScale = originalGravityScale; // 원래 중력으로 복구
        }

        if (Input.GetButtonDown("Jump") && isWall && wallSliding && !isGround && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            wallJump = true;
            lastWallJumpTime = Time.time;

            wallJumped = facingRight;
            
            rigid.velocity = new Vector2(PlayerManager.Instance.playerStatus.speed * -horizontal, PlayerManager.Instance.playerStatus.jumpPower);
            Invoke("WallJumpCheck", wallJumpCooldown);

            
        }
    }

    private void SlidingWall()
    {
        if (wallSliding && !wallJump)
        {
            animator.SetBool("isWallSliding", true);
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Clamp(rigid.velocity.y, -PlayerManager.Instance.playerStatus.jumpPower * 0.5f, float.MaxValue));
        }
    }

    private void CheckBoolType()
    {
        isGround = Physics2D.Raycast(checkGround.position, Vector2.down, 1.1f, ground);
        isWall = Physics2D.Raycast(checkWall.position, facingRight ? Vector2.right : Vector2.left, 0.4f, wall);

        if (isGround)
        {
            animator.SetBool("isGround", true);
            
            doubleJump = true;
            wallJump = false;
        }
        else
        {
            animator.SetBool("isGround", false);
        }

        if(isGround && rigid.velocity.y == 0){
            animator.SetBool("isJump", false);
            animator.SetBool("isDouble", false);
        }

        if (isWall && !isGround && !wallJump)
        {
            animator.SetBool("isJump", true);
            wallSliding = true;
        }
        else if (!isWall || isGround)
        {
            animator.SetBool("isWallSliding", false);
            wallSliding = false;
        }
    }

    private void StopSliding()
    {
        if (wallSliding && horizontal < 0 && facingRight)
        {
            wallSliding = false;
        }
        else if (wallSliding && horizontal > 0 && !facingRight)
        {
            wallSliding = false;
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontal < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void WallJumpCheck()
    {
        wallJump = false;
    }
}

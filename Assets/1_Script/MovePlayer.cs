using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : PlayerStatus
{

    private float downPower = 6f;
    private float horizontal;

    private float wallJumpCooldown = 0.2f; // 벽 점프 쿨다운 시간
    private float lastWallJumpTime;

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

    private void Start()
    {
        checkGround = transform.Find("CheckGround");
        checkWall = transform.Find("CheckWall");

        rigid = GetComponent<Rigidbody2D>();
        gravityVec = new Vector2(0, -Physics2D.gravity.y);
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
            float targetSpeed = horizontal * speed;
            rigid.velocity = new Vector2(Mathf.Lerp(rigid.velocity.x, targetSpeed, 0.6f), rigid.velocity.y);
            Flip(horizontal);
        }
    }

    private void PlayerJump()
    {
        // 그냥 점프 + 더블 점프
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
             
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            }
            else if (doubleJump)
            {
                doubleJump = false;
               
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            }
        }
        if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigid.gravityScale = 2f;
        }
       
        if (rigid.velocity.y < 0)
        {
            rigid.velocity -= gravityVec * downPower * Time.deltaTime;
        }

        // 벽 점프
        if (Input.GetButtonDown("Jump") && isWall && wallSliding && !isGround && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            wallJump = true;
            lastWallJumpTime = Time.time; // 벽 점프 시간을 갱신

            // 벽 점프 방향 갱신
            wallJumped = facingRight;

            // 벽 점프 속도 설정
            rigid.velocity = new Vector2(speed * -horizontal, jumpPower);

            Invoke("WallJumpCheck", wallJumpCooldown);
        }
    }

    private void SlidingWall()
    {
        if (wallSliding && !wallJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Clamp(rigid.velocity.y, -jumpPower * 0.5f, float.MaxValue));
        }
    }

    private void CheckBoolType()
    {
        isGround = Physics2D.Raycast(checkGround.position, Vector2.down, 1.1f, ground);
        isWall = Physics2D.Raycast(checkWall.position, facingRight ? Vector2.right : Vector2.left, 0.4f, wall);

        if (isGround)
        {
            doubleJump = true;
            wallJump = false;
        }

        if (isWall && !isGround && !wallJump)
        {
            wallSliding = true;
        }
        else if (!isWall || isGround)
        {
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

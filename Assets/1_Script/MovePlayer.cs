using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{


    public float speed;
    public float jumpPower;
    private float downPower = 4f;
    private float horizontal;


    private bool isGround;
    private bool isWall;
    private bool wallSliding;
    private bool facingRight = true;

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
        PlayerMove();
        PlayerJump();
        SlidingWall();

        CheckBoolType();
    }

    private void PlayerMove()
    {
        if (!wallSliding)
        {
            horizontal = Input.GetAxis("Horizontal");
            float targetSpeed = horizontal * speed;
            rigid.velocity = new Vector2(Mathf.Lerp(rigid.velocity.x, targetSpeed, 0.6f), rigid.velocity.y);
            Flip(horizontal);
        }
    }

    private void PlayerJump()
    {

        if (isGround && Input.GetButtonDown("Jump"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
        if(rigid.velocity.y < 0)
        {
            rigid.velocity -= gravityVec * downPower * Time.deltaTime;
        }
    }

    private void SlidingWall()
    {
        if(wallSliding)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Clamp(rigid.velocity.y, -jumpPower, float.MaxValue));
        }
    }
    private void CheckBoolType()
    {
        isGround = Physics2D.Raycast(checkGround.position, Vector2.down, 1.1f, ground);
        isWall = Physics2D.Raycast(checkWall.position, Vector2.right, 0.3f, wall);

        if(isWall && !isGround && horizontal != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.y = Mathf.Abs(theScale.y);
            transform.localScale = theScale;
        }
        else if (horizontal < 0 && facingRight)
        {
            facingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.y = -Mathf.Abs(theScale.y);
            transform.localScale = theScale;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;
    private AnimatorStateInfo stateInfo;
    public CameraMove cam;

    private float downPower = 2f;
    public float horizontal = 0;

    private float wallJumpCooldown = 0.6f; // 벽 점프 쿨다운 시간
    private float lastWallJumpTime;


    public bool isMove;
    public bool isJump;
    private bool isEnemy = false;
    public bool isGround;
    public bool isWall;
    private bool wallJump;
    private bool wallSliding;
    private bool doubleJump;
    public bool facingRight = true;
    private bool wallJumped; // 벽 오른쪽 확인

    private Transform checkGround;
    private Transform checkWall;

    public LayerMask ground;
    public LayerMask wall;

    public Rigidbody2D rigid;

    private Vector2 gravityVec;
    public bool canMove = true;

    private float originalGravityScale; // 원래 중력 값 저장

    public AudioClip moveClip;
    public AudioClip jumoClip;
    public AudioClip doubleClip;
    public AudioClip Clip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        checkGround = transform.Find("CheckGround");
        checkWall = transform.Find("CheckWall");
        check = GetComponent<PlayerCheckMonster>();
        cam = Camera.main.GetComponent<CameraMove>();
        rigid = GetComponent<Rigidbody2D>();
        gravityVec = new Vector2(0, -Physics2D.gravity.y);
        originalGravityScale = rigid.gravityScale; // 시작할 때 중력 값을 저장
    }

    private void Update()
    {
        CheckBoolType();
        StopSliding();
        PlayerMove();
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveLeftButtonDown();
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            StopMoving();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightButtonDown();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            StopMoving();
        }
        if (Input.GetButtonDown("Jump"))
        {
            PlayerJump();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Stage1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Stage2");
        }
        GetDeath();
        SlidingWall();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("Skil4", true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("Skil4", false);
        }
    }
    public void MoveLeftButtonDown()
    {
        if (canMove)
        {
            isMove = true;
            //SoundManager.Instance.CURSound(isMove, moveClip);
            horizontal = -1;
        }
       
    }

    public void MoveRightButtonDown()
    {
        if (canMove)
        {
            isMove = true;
            //SoundManager.Instance.CURSound(isMove, moveClip);
            horizontal = 1;
        }

    }

    public void StopMoving()
    {
        isMove = false;
        //SoundManager.Instance.CURSound(isMove);
        horizontal = 0;
    }
    private void PlayerMove()
    {
        //horizontal = Input.GetAxis("Horizontal");
        

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

    //private void PlayerJump()
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        if (isGround || isEnemy)
    //        {
    //            isEnemy = false;
    //            rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
    //            animator.SetBool("isJump", true) ;
    //            animator.SetBool("isGround", false);



    //        }
    //        else if (doubleJump)
    //        {
    //            doubleJump = false;
    //            rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
    //            animator.SetBool("isJump", false);
    //            animator.SetBool("isDouble", true ) ;

    //        }
    //    }

    //    if (rigid.velocity.y > 0)
    //    {
    //        rigid.gravityScale = originalGravityScale * 1.5f; // 상승 속도를 더 빠르게
    //    }
    //    else if (rigid.velocity.y < 0)
    //    {

    //        rigid.gravityScale = originalGravityScale * 2f; // 하강 속도를 더 빠르게
    //        rigid.velocity -= gravityVec * downPower * Time.deltaTime;
    //    }
    //    else
    //    {
    //        rigid.gravityScale = originalGravityScale; // 원래 중력으로 복구
    //    }

    //    if (Input.GetButtonDown("Jump") && isWall && wallSliding && !isGround && Time.time > lastWallJumpTime + wallJumpCooldown)
    //    {
    //        wallJump = true;
    //        lastWallJumpTime = Time.time;

    //        wallJumped = facingRight;

    //        rigid.velocity = new Vector2(PlayerManager.Instance.playerStatus.speed * -horizontal, PlayerManager.Instance.playerStatus.jumpPower);
    //        Invoke("WallJumpCheck", wallJumpCooldown);

    //        StartCoroutine(cam.CamMover(facingRight));
    //    }
    //}
    public void PlayerJump()
    {
        if (isGround || isEnemy)
        {
            // 일반 점프
            isJump = true;
            RegularJump();
        }
        else if (doubleJump)
        {
            // 더블 점프

            DoubleJump();
        }
        else if (isWall && wallSliding && !isGround && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            // 벽 점프
            WallJump();
        }
    }
    private void RegularJump()
    {
        isEnemy = false;
        //SoundManager.Instance.CURSound(false);
        SoundManager.Instance.SFXSound("Reaular",jumoClip );
        rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
        animator.SetBool("isJump", true);
        animator.SetBool("isGround", false);
    }

    private void DoubleJump()
    {
        doubleJump = false;
        SoundManager.Instance.SFXSound("Double", doubleClip);
        rigid.velocity = new Vector2(rigid.velocity.x, PlayerManager.Instance.playerStatus.jumpPower);
        animator.SetBool("isJump", false);
        animator.SetBool("isDouble", true);
    }

    private void WallJump()
    {
        wallJump = true;
        lastWallJumpTime = Time.time;

        wallJumped = facingRight;
        SoundManager.Instance.SFXSound("Reaular", jumoClip);
        rigid.velocity = new Vector2(PlayerManager.Instance.playerStatus.speed * -horizontal, PlayerManager.Instance.playerStatus.jumpPower);
        Invoke("WallJumpCheck", wallJumpCooldown);
        StartCoroutine(cam.CamMover(facingRight));
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
            isJump = false;

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

        if (stateInfo.IsName("PlayerDown"))
        {
            StartCoroutine(cam.Shake(0.01f, 0.01f, 0.7f));
        }
    }

    private PlayerCheckMonster check;
    private void GetDeath()
    {
        if(rigid.velocity.y <=  -100)
        {
            check.deathPlayer += () => true;
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

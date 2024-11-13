using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finding : MonoBehaviour
{
   
    public delegate void NormalAttack();
    public event NormalAttack normal;

    public bool isCanFind = true;
    public bool isRight;
    private bool attackTrue = false;

    public Animator animator;
    public Transform player;
    public Transform check;
    public Rigidbody2D rb;
    public MovePlayer mover;

    public Boss1 boss;
    private Vector2 origin;

    private float tele = 5; 


    private float disy = 11; 
    private bool canTeleport = true;  
    private bool isGround;
    public bool isWalk;

    public AudioClip teleClip;
    public AudioClip trigClip;

    public LayerMask mask;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        check = player.Find("CheckGround").transform;
        mover = player.gameObject.GetComponent<MovePlayer>();
        boss = GetComponent<Boss1>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AttackTrueCool());
    }

    private void Update()
    {
        if(isCanFind)
        {
            if (boss.isMove)
            {
                Check();
            }
            if (attackTrue)
            {
                normal?.Invoke();
                StartCoroutine(AttackTrueCool());
            }
        }
    }

    private void Check()
    {
        isGround = Physics2D.Raycast(check.position, Vector2.down, 1.3f, mask);
      
        if(transform.position.x < player.transform.position.x)
        {
    
            isRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        if(transform.position.x > player.transform.position.x)
        {
            
            isRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        if(Vector2.Distance(player.transform.position, transform.position) < 8)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if(isWalk)
            {
 
                animator.SetBool("Side", true);
            }
            Vector2 target = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(target.x * boss.speed, rb.velocity.y);

        }



        if (player.position.y > disy && isGround && canTeleport && Vector2.Distance(player.position, transform.position) > 31 && canTeleport && isGround)
        {
            SoundManager.Instance.BossSound("Tel", teleClip);
            TeleportBehindPlayer();

        }

    }

    private void TeleportBehindPlayer()
    {
        canTeleport = false;
        StartCoroutine(GetAnimator());
        SoundManager.Instance.BossSound("Trig", trigClip);
        if (mover.facingRight)
        {

            transform.position = new Vector3(player.position.x - 4f, player.position.y + 3);
        }
        else
        {

            transform.position = new Vector3(player.position.x + 4f, player.position.y + 3);
        }

        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(tele);
        canTeleport = true; 
    }
    private IEnumerator AttackTrueCool()
    {
        attackTrue = false;
        yield return new WaitForSeconds(1.3f);
        attackTrue = true;
    }

    private IEnumerator GetAnimator()
    {
        isCanFind = false;
        isWalk = false;
        animator.SetBool("Side", false);
        animator.SetBool("isTel", true);
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        while (state.normalizedTime < 1.0f)
        {
            state = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; // 매 프레임마다 대기
        }
        isWalk = true;
        animator.SetBool("isTel", false);
        isCanFind = true;
    }

}

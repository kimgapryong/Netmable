using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finding : MonoBehaviour
{
   
    public delegate void NormalAttack();
    public event NormalAttack normal;

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
        if(boss.isMove)
        {
            
            Check();
        }
        if(attackTrue)
        {
            normal?.Invoke();
            StartCoroutine(AttackTrueCool());
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
            Debug.Log(player.position);
            TeleportBehindPlayer();
        }

    }

    private void TeleportBehindPlayer()
    {
        canTeleport = false; 

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
}

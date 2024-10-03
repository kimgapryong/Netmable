using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Finding;

public class Finding : MonoBehaviour
{

    public delegate void NormalAttack(Vector2 vector);
    public event NormalAttack normal;

    public bool isRight;
    private bool attackTrue = true;

    public Transform player;
    public Transform check;
    public Rigidbody2D rb;
    public MovePlayer mover;

    public Boss1 boss;

    private float tele = 5; 


    private float disy = 11; 
    private bool canTeleport = true;  
    private bool isGround;


    public LayerMask mask;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        check = player.Find("CheckGround").transform;
        mover = player.gameObject.GetComponent<MovePlayer>();
        boss = GetComponent<Boss1>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(boss.isMove)
        {
            
            Check();
        }
        if(attackTrue)
        {
            normal?.Invoke(player.transform.position);
            StartCoroutine(AttackTrueCool());
        }
    }

    private void Check()
    {
        isGround = Physics2D.Raycast(check.position, Vector2.down, 1.3f, mask);
      
        if(transform.position.x <= player.transform.position.x)
        {
            isRight = true;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
        }
        else if(transform.position.x > player.transform.position.x)
        {
            isRight = false;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }

        if(Vector2.Distance(player.transform.position, transform.position) < 8)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
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
        yield return new WaitForSeconds(0.7f);
        attackTrue = true;
    }
}

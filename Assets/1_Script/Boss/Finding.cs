using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finding : MonoBehaviour
{
    public Transform player;
    public Transform check;
    public Rigidbody2D rb;
    public MovePlayer mover;

    public Boss1 boss;

    private float tele = 5;  // 순간이동 쿨타임
    private float dis = 15;  // 보스가 플레이어를 추적하는 거리
    private float disy = 11;  // 플레이어의 y축 위치 기준
    private bool canTeleport = true;  // 순간이동 가능 여부
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
        
    }

    private void Check()
    {
        isGround = Physics2D.Raycast(check.position, Vector2.down, 1.3f, mask);
        if(Vector2.Distance(player.position, transform.position) < dis)
        {

        }
        else
        {
            Vector2 target = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(target.x * boss.speed, rb.velocity.y);
        }
       
        if(player.position.y > disy && isGround && canTeleport || Vector2.Distance(player.position, transform.position) > 41 && canTeleport && isGround)
        {
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
}

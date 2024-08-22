using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Supporter : MonoBehaviour
{
    public int damage;
    public int speed = 6;

    public GameObject attackBool;

    public float follwDistance = 3;
    
    public float attackCool = 1f;
    private Vector3 offset = new Vector3(-2f, 1.5f, 0f);
    private Vector3 offsets = new Vector3(2f, 1.5f, 0f);

    private bool canAttack = true;
    private bool fallow = true;
    public bool isRight;

    private Vector2 transPos;
    private MovePlayer movePlayer;
    private Transform player;
    private MonsterManager monster;

    private void Start()
    {
        monster = FindObjectOfType<MonsterManager>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        movePlayer = player.GetComponent<MovePlayer>();

    }

    private void Update()
    {
        FollowPlayer();
        MonsterCheck();
        Debug.Log(fallow);
    }
    private void FollowPlayer()
    {
        if (fallow)
        {
            if (movePlayer.facingRight)
            {
                isRight = true;
                Vector3 targetPosition = player.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                isRight = false;
                Vector3 targetPosition = player.position + offsets;
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(180f,0f, 0f);
            }
            
        }
        transPos = transform.position;
    }

    private void MonsterCheck()
    {
        foreach(Monster mons in monster.monsters)
        {
            if(mons != null)
            {
                if(Vector2.Distance(player.position, mons.transform.position) < 16)
                {
                    if (player.position.x > mons.transform.position.x)
                    {
                        player.position = new Vector2(player.position.x - 21, player.position.y);
                    }
                }
               
                if (Vector2.Distance(mons.transform.position, transform.position) <= 35f && transform.position.y >= mons.transform.position.y && transform.position.y <= mons.transform.position.y + 2)
                {
                    
                    movePlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    movePlayer.enabled = false;
                    if (canAttack)
                    {
                        canAttack = false;
                        fallow = false;
                        StartCoroutine(TransMove(mons));

                    }
                }
            }
            
        }
    }

    private IEnumerator TransMove(Monster mons)
    {
        Vector2 mosPos = mons.transform.position;
        if (!fallow)
        {
            while (Vector2.Distance(transform.position, mosPos) > 20f)
            {
                transform.position = Vector2.MoveTowards(transform.position, mosPos, (speed + 41) * Time.deltaTime);
                yield return null;
            }

            MonsterAttack();
            yield return new WaitForSeconds(1);
            while (Vector2.Distance(transform.position, transPos) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, transPos, (speed + 10) * Time.deltaTime);
                yield return null;
            }

            fallow = true;
   
        }

    }

    private void MonsterAttack()
    {
        GameObject clone = Instantiate(attackBool, transform.position + new Vector3(4, 0, 0), Quaternion.identity);
        clone.GetComponent<AttackBoolTest>().sup = this;
    }

}
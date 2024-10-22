using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private CameraMove cam;

    public List<Monster> monsters = new List<Monster>();

    public delegate void onDestroyMonster(List<Monster> monster);
    public event onDestroyMonster onDestroy;

    private void Start()
    {
        monster = FindObjectOfType<MonsterManager>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        movePlayer = player.GetComponent<MovePlayer>();
        cam = Camera.main.GetComponent<CameraMove>();
    }

    private void Update()
    {
        FollowPlayer();
        if(onDestroy != null)
        {
            Debug.Log(monsters);
            onDestroy?.Invoke(monsters);
        }
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
                transform.localScale = new Vector3(1, 1, 1);

            }
            else
            {
                isRight = false;
                Vector3 targetPosition = player.position + offsets;
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
        transPos = transform.position;
    }

    public void MonsterCheck()
    {
      

        foreach(Monster mons in monster.monsters)
        {
            if (mons != null)
            {
                monsters.Add(mons);
            }
            
        }
        Monster firstMon = CheckLength();
        Debug.Log(firstMon);    
        movePlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        movePlayer.enabled = false;
        if (canAttack)
        {
            canAttack = false;
            fallow = false;
            StartCoroutine(TransMove(firstMon));
            cam.StartFollowing(transform);
        }
    }
    private Monster CheckLength()
    {
        float lower = Vector2.Distance(transform.position, monsters[0].transform.position);
        Monster mons = monsters[0];
        foreach (Monster mon in monsters)
        {
            float lowDis = Vector2.Distance(transform.position, mon.transform.position);
            if (lowDis <= lower)
            {
                Debug.Log("몬스터 거리:" + lower + " " + "몬스터:" + mons);
                lower = lowDis;
                mons = mon;
                
            }
        }
        Debug.Log("몬스터 거리:" + lower + " " + "몬스터:" + mons);
        return mons;
    }
    private IEnumerator TransMove(Monster mons)
    {
        Vector2 mosPos = mons.transform.position;
        if (!fallow)
        {
            while (Vector2.Distance(transform.position, mosPos) > 10f)
            {
                transform.position = Vector2.MoveTowards(transform.position, mosPos, (speed + 20) * Time.deltaTime);
                yield return null;
            }

            MonsterAttack();
            yield return new WaitForSeconds(1);
            while (Vector2.Distance(transform.position, transPos) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, transPos, (speed + 10) * Time.deltaTime);
                yield return null;
            }
            if (mons != null)
            {
                Destroy(mons.gameObject);
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster04 : Monster
{

    
    public MonsterData data;
    public float monDis = 40f;
    public float monAtt = 20f;
    public float bulletSpeed = 60;
   
    private Vector2 length;
    private Rigidbody2D rb;

    public Transform fireHole;
    public GameObject bullet;
    private GameObject clone;
    private bool isAtk = true;
    private bool arg;
    private void Start()
    {
        ResetData(data);
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        MonsterMove();
        MonsterSkils();
        bulletShoot();
        Check();
        
    }
    public override void Check()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            if (player.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
            }
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y));
            }
        }
    }
    protected override void MonsterMove()
    {
        length = (player.transform.position - transform.position).normalized;
        if (Vector2.Distance(player.transform.position, transform.position) <= monAtt)
        {
            arg = true; 
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(length.y, length.x) * Mathf.Rad2Deg);
        }
        else if (Vector2.Distance(player.transform.position, transform.position) <= monDis)
        {
            arg = false;
            rb.velocity = new Vector2(length.x, 0) * speed;
        }
        
    }

    protected override void MonsterSkils()
    {
        if(arg)
        {
            if(isAtk)
            {
                isAtk = false;
                clone = Instantiate(bullet, fireHole.transform.position, Quaternion.identity);
                StartCoroutine(waitAtk());
            }
        }
    }
    private void bulletShoot()
    {
        if(clone != null)
        {
            clone.transform.Translate(length * bulletSpeed * Time.deltaTime);
            Destroy(clone, 2.5f);
        }
    }
    private IEnumerator waitAtk()
    {
        yield return new WaitForSeconds(2f);
        isAtk = true;
    }
   
}

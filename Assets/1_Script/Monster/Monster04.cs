using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster04 : Monster
{

    
    public MonsterData data;
    public Animator animator;
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

    public AudioClip dieClip;
    private void Start()
    {
        ResetData(data);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
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

            animator.SetBool("isAtk", true);
            arg = true; 
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(length.y, length.x) * Mathf.Rad2Deg);
        }
        else if (Vector2.Distance(player.transform.position, transform.position) <= monDis)
        {
            animator.SetBool("isAtk", false);
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
                SoundManager.Instance.SFXSound("Mon4", monClip);
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

    public override void TakeDamage(int attack)
    {
        base.TakeDamage(attack);
        if (paticle != null && normalAttack)
        {
            GameObject clone = Instantiate(paticle, transform.position, Quaternion.identity);
            Destroy(clone, 0.3f);
        }

        health -= attack;

        if (health <= 0)
        {
            isAtk = false ;
            animator.SetTrigger("isDie");
            Die();
        }
    }
    private bool isItem = true;
    protected override void Die()
    {
        if (!isOk) return;
        SoundManager.Instance.SFXSound("Mon4Die", dieClip);
        PlayerManager.Instance.playerStatus.AddLevel(Exp);
        if (ItemManager.Instance != null && isItem)
        {
            isItem = false;
            ItemManager.Instance.RandomItem(gameObject.transform.position);
        }


        if (UiManager.Instance != null && UiManager.Instance.mSlider != null)
        {
            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(false);
        }


        Destroy(gameObject, 1.6f);
    }
}

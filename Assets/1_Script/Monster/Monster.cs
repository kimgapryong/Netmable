using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public GameObject player;
    public GameObject paticle;
    public MonsterData monsterData;

    public Vector2 rightCheck;
    public Sprite monsterSprite;
    public string monsterName;

    public int Exp;
    public int MaxHp;
    public int health;
    public float speed;
    public int damage;
    public bool isAttack;
    public bool isOk = true;

    public bool normalAttack = true;
    private bool attackTime;
    public AudioClip monClip;
    public void ResetData(MonsterData monsterData)
    {
        
        player = GameObject.Find("Player");
        this.monsterData = monsterData;
        Exp = monsterData.Exp;
        MaxHp = monsterData.health;
        monsterSprite = monsterData.monsterSprite;
        monsterName = monsterData.monsterName;
        health = monsterData.health;
        speed = monsterData.speed;
        damage = monsterData.damage;
        isAttack = monsterData.isAttack;
        monClip = monsterData.monsterClip;
    }
    protected virtual void Update()
    {
        if (isOk)
        {
            MonsterMove();
            MonsterSkils();
            Check();
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skil") || collision.CompareTag("Bullet"))
        {
            attackTime = false;
            Renderer ren = gameObject.GetComponent<Renderer>();
            StartCoroutine(RendererON(ren));

            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(true);
            UiManager.Instance.StartMonsterUiCoroutine(monsterData, MaxHp, () => health);
       
        }
    }
    public virtual void Check()
    {
        if(player != null)
        {
            if (player.transform.position.x <= transform.position.x)
            {
                rightCheck = Vector2.left;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            if (player.transform.position.x > transform.position.x)
            {
                rightCheck = Vector2.right;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        
    }
    private IEnumerator RendererON(Renderer ren)
    {
        if (ren != null)
        {
            
            ren.enabled = false;
            yield return new WaitForSeconds(0.2f);
            ren.enabled = true;
            yield return new WaitForSeconds(0.3f);
          
            attackTime = true;
            normalAttack = true;
        }
        else
        {
            Debug.Log("¿Ö ¶Ç ¾ÈµÅ");
        }
    }
    public virtual void TakeDamage(int attack)
    {
        Debug.Log(attack);
        //if (paticle != null && normalAttack)
        //{
        //    GameObject clone = Instantiate(paticle, transform.position, Quaternion.identity);
        //    Destroy(clone, 0.3f);
        //}
        
        //health -= attack;
        
        //if (health <= 0)
        //{
            
        //    Die();
        //}
    }
    protected virtual void Die()
    {
        if (!isOk) return;
        PlayerManager.Instance.playerStatus.AddLevel(Exp);
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RandomItem(gameObject.transform.position);
        }


        if (UiManager.Instance != null && UiManager.Instance.mSlider != null)
        {
            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(false);
        }


        Destroy(gameObject, 0.1f);
    }

    protected abstract void MonsterMove();
    protected abstract void MonsterSkils();

}

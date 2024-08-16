using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public GameObject player;
    public GameObject paticle;
    public MonsterData monsterData;

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
    private void Check()
    {
        if(player != null)
        {
            if (player.transform.position.x < transform.position.x)
            {
               transform.rotation = Quaternion.Euler(0,0,0);
            }
            else if (player.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
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
    public void TakeDamage(int attack)
    {

        if (paticle != null && normalAttack)
        {
            GameObject clone = Instantiate(paticle, transform.position, Quaternion.identity);
            Destroy(clone, 0.3f);
        }
        
        health -= attack;
        
        if (health <= 0)
        {
            
            Die();
        }
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

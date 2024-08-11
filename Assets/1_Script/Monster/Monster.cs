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
    private void Update()
    {
        if (isOk)
        {
            MonsterMove();
            MonsterSkils();
            Check();
        }
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            attackTime = false;
            Renderer ren = gameObject.GetComponent<Renderer>();
            StartCoroutine(RendererON(ren));



            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(true);
            UiManager.Instance.StartMonsterUiCoroutine(monsterData, MaxHp, () => health);


            TakeDamage(PlayerManager.Instance.playerStatus.damage);
            Destroy(collision.gameObject);

        }
    }
    private void Check()
    {
        if(player != null)
        {
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        
    }
    private IEnumerator RendererON(Renderer ren)
    {
        if (ren != null)
        {
            
            ren.enabled = false;
            yield return new WaitForSeconds(0.3f);
            ren.enabled = true;
            yield return new WaitForSeconds(0.3f);
          
            attackTime = true;
        }
    }
    public void TakeDamage(int attack)
    {

        if (paticle != null)
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
        isOk = false;
        PlayerManager.Instance.playerStatus.AddLevel(Exp);
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RandomItem(gameObject.transform.position);
        }

        // UiManager가 유효한지 확인
        if (UiManager.Instance != null && UiManager.Instance.mSlider != null)
        {
            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(false);
        }

        Destroy(gameObject, 0.1f);
    }

    protected abstract void MonsterMove();
    protected abstract void MonsterSkils();

}

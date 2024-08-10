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

    public int MaxHp;
    public int health;
    public float speed;
    public int damage;
    public bool isAttack;

    private bool attackTime;



    public void ResetData(MonsterData monsterData)
    {
        
        player = GameObject.Find("Player");
        this.monsterData = monsterData;
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
        MonsterMove();
        MonsterSkils();
        Check();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            attackTime = false;
            Renderer ren = gameObject.GetComponent<Renderer>();
            StartCoroutine(RendererON(ren));

            UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(true);
            StartCoroutine(UiManager.Instance.UpdateMonsterUi(monsterData,MaxHp, () => health));


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
        
        health -= attack;
        GameObject clone = Instantiate(paticle, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            ItemManager.Instance.RandomItem(gameObject.transform.position);
            Die();
        }
        Destroy(clone, 0.4f);
    }
    protected virtual void Die()
    {
        UiManager.Instance.mSlider.transform.parent.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    protected abstract void MonsterMove();
    protected abstract void MonsterSkils();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public GameObject player;
    public string monsterName;
    public int health;
    public float speed;
    public float damage;
    public bool isAttack;

    private bool attackTime;



    public void ResetData(MonsterData monsterData)
    {
        player = GameObject.Find("Player");
        monsterName = monsterData.monsterName;
        health = monsterData.health;
        speed = monsterData.speed;
        damage = monsterData.damage;
        isAttack = monsterData.isAttack;
    }
    private void Update()
    {
        MonsterMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            attackTime = false;
            Renderer ren = gameObject.GetComponent<Renderer>();
            StartCoroutine(RendererON(ren));
            TakeDamage(PlayerManager.Instance.bulletMagic.damage);
            Destroy(collision.gameObject);

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
        if (health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log("Á×À½");
        Destroy(gameObject);
    }

    protected abstract void MonsterMove();
    protected abstract void MonsterSkils();

}

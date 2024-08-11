using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //[SerializeField]
    //private GameObject bullet;

    //[SerializeField]
    //private float attackCooldown = 0.5f;

    //private float lastAttackTime; 

    //private void Start()
    //{
    //    lastAttackTime = -attackCooldown;
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
    //    {
    //        Attack();
    //    }
    //}

    //private void Attack()
    //{
    //    Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -90));
    //    lastAttackTime = Time.time;
    //}

    public Collider2D attackCollider; 
    public float attackRate = 2f; 
    private float nextAttackTime = 0f;

    //public Animator animator; 
    private PlayerStatus playerStatus; 
    private bool canAttack = true;
    void Start()
    {
        attackCollider.enabled = false; 
        playerStatus = GetComponent<PlayerStatus>(); 
    }

    void Update()
    {
        // 공격 입력 감지 및 쿨타임 체크
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
   
        canAttack = false;
        attackCollider.enabled = true; 
        //animator.SetTrigger("Attack"); // 공격 애니메이션 실행

        // 쿨타임 설정
        nextAttackTime = Time.time + 1f / attackRate;

        yield return new WaitForSeconds(0.1f); 

        attackCollider.enabled = false; 
        yield return new WaitForSeconds(1f / attackRate); 

        canAttack = true; 
    }

    // Trigger 이벤트 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
        
            collision.GetComponent<Monster>().TakeDamage(playerStatus.damage);
        }
    }
}

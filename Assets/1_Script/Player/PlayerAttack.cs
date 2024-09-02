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

    //private MovePlayer mover;
    //public Collider2D attackCollider;
    //private PlayerCheckMonster monsterCheck;

    //public float attackRate = 2f; 
    //private float nextAttackTime = 0f;

    ////����
    //private Animator animator;
    //private GameObject monster;
    ////public Animator animator; 
    //private PlayerStatus playerStatus; 
    //private bool canAttack = true;
    //void Start()
    //{
    //    mover = GetComponent<MovePlayer>();
    //    attackCollider.enabled = false; 
    //    playerStatus = GetComponent<PlayerStatus>(); 
    //    monsterCheck = GetComponent<PlayerCheckMonster>();  
    //}

    //void Update()
    //{
    //    // ���� �Է� ���� �� ��Ÿ�� üũ
    //    if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime && canAttack)
    //    {
    //        StartCoroutine(PerformAttack());
    //    }
    //}

    //private IEnumerator PerformAttack()
    //{

    //    canAttack = false;
    //    attackCollider.enabled = true; 
    //    monsterCheck.enabled = false;
    //    //animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ����

    //    // ��Ÿ�� ����
    //    nextAttackTime = Time.time + 1f / attackRate;

    //    yield return new WaitForSeconds(0.1f); 

    //    attackCollider.enabled = false; 
    //    monsterCheck.enabled = true;
    //    yield return new WaitForSeconds(1f / attackRate); 

    //    if(monster != null && animator != null)
    //    {
    //        animator.enabled = true;
    //    }

    //    canAttack = true; 
    //}

    //// Trigger �̺�Ʈ ó��
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        monster = collision.gameObject;
    //        collision.GetComponent<Monster>().TakeDamage(playerStatus.damage);
    //        animator = collision.GetComponent<Animator>();
    //        if (monster != null && animator != null)
    //        {
    //            animator.enabled = false;
    //        }
    //        Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
    //        if (mover.facingRight)
    //        {
    //            rigid.velocity = new Vector2(5f, 0);
    //        }
    //        else
    //        {
    //            rigid.velocity = new Vector2(-5f, 0);
    //        }

    //    }
    //}

    private MovePlayer mover;
    private PlayerStatus status;
    public CameraMove cam;
    

    private bool checkAttack;
    private float timer = 0.2f;
    private float times = 0.0f;
    public Transform trans;
    public Vector2 vec2;

    private float normalizedTime;
    private void Start()
    {
        mover = GetComponent<MovePlayer>();
        status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        AttackPlayer();

        Check();
    }

    private void AttackPlayer()
    {
        if(times >= timer)
        {
            if(Input.GetMouseButton(0) && mover.isGround)
            {
                if (checkAttack)
                {
                    checkAttack = false;
                    mover.animator.SetTrigger("Attack1");
                    mover.animator.ResetTrigger("Attack2");  // Attack2 Ʈ���� ����
                }
                else
                {
                    checkAttack = true;
                    mover.animator.SetTrigger("Attack2");
                    mover.animator.ResetTrigger("Attack1");  // Attack1 Ʈ���� ����
                }
                mover.enabled = false;
                Collider2D[] colider = Physics2D.OverlapBoxAll(trans.position, vec2, 0);
                foreach(Collider2D colider2d in colider)
                {
                  if(colider2d != null)
                    {
                        Monster monster = colider2d.GetComponent<Monster>();
                        if (monster != null)
                        {
                            monster.TakeDamage(status.damage);
                            StartCoroutine(cam.Shake(0.1f, 0.15f, 0.5f));
                        }
                    }
                }
               
                times = 0.0f;
            }
        }
        else
        {
            times += Time.deltaTime;
        }
    }

    private void Check()
    {
        AnimatorStateInfo stateInfo = mover.animator.GetCurrentAnimatorStateInfo(0);
        normalizedTime = stateInfo.normalizedTime % 1;  // 0���� 1 ������ ������ ����ȭ
        if (mover.enabled == false && normalizedTime > 0.95f && (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2")))
        {
            mover.enabled = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(trans.position, vec2);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private MovePlayer mover;
    private Animator monAnime;
    private PlayerStatus status;
    private PlayerCheckMonster checkMonster;
    public CameraMove cam;

    private bool checkAttack;
    private bool canAttack = true;
    private float timer = 0.2f;
    private float times = 0.0f;
    public Transform trans;

    private Vector2 playVec;
    private Vector2 originVec;
    public Vector2 vec2;
    private Transform originalParent; // trans의 원래 부모

    private void Start()
    {
        mover = GetComponent<MovePlayer>();
        status = GetComponent<PlayerStatus>();
        checkMonster = GetComponent<PlayerCheckMonster>();
        originalParent = trans.parent; // 부모 저장
    }

    private void Update()
    {
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        if (times >= timer && canAttack)
        {
            if (Input.GetMouseButton(0) && mover.isGround)
            {
                canAttack = false; // 다른 애니메이션 실행을 막음
                originVec = trans.position;
                playVec = transform.position;

                // 부모 관계를 일시적으로 끊음
                trans.SetParent(null);

                if (checkAttack)
                {
                    checkAttack = false;
                    mover.animator.SetTrigger("Attack1");
                    mover.animator.ResetTrigger("Attack2");
                    StartCoroutine(EnableMovementAfterDelay(mover.animator.GetCurrentAnimatorStateInfo(0).length));
                }
                else
                {
                    checkAttack = true;
                    mover.animator.SetTrigger("Attack2");
                    mover.animator.ResetTrigger("Attack1");
                    StartCoroutine(EnableMovementAfterDelay(mover.animator.GetCurrentAnimatorStateInfo(0).length));
                }

                mover.enabled = false;
                checkMonster.enabled = false;

                Collider2D[] colider = Physics2D.OverlapBoxAll(trans.position, vec2, 0);
                foreach (Collider2D colider2d in colider)
                {
                    if (colider2d != null)
                    {
                        Monster monster = colider2d.GetComponent<Monster>();
                        if (monster != null)
                        {
                            monster.TakeDamage(status.damage);
                            monAnime = monster.GetComponent<Animator>();
                            if (monster != null && monAnime != null)
                            {
                                monAnime.enabled = false;
                            }
                            Rigidbody2D rigid = monster.GetComponent<Rigidbody2D>();
                            if (mover.facingRight)
                            {
                                rigid.velocity = new Vector2(5f, 0);
                            }
                            else
                            {
                                rigid.velocity = new Vector2(-5f, 0);
                            }
                            //StartCoroutine(cam.Shake(0.1f, 0.15f, 0.5f));
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

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(0.2f);
        mover.enabled = true;
        yield return new WaitForSeconds(delay - 0.25f);
        canAttack = true; // 애니메이션이 끝난 후 공격 가능 상태로 전환
        checkMonster.enabled = true;
        if (monAnime != null)
        {
            monAnime.enabled = true;
        }

        // 공격이 끝난 후 부모 관계를 다시 설정
        trans.SetParent(originalParent);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(trans.position, vec2);
    }
}

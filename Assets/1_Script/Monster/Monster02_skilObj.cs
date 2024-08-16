using System.Collections;
using UnityEngine;

public class Monster02_skilObj : MonoBehaviour
{
    private GameObject player;

    public Transform monster;  // 오브젝트가 생성될 위치
    public Monster02 monsters;

    private bool isCool = true;
    private bool isLeftAttack;  // 공격 방향을 저장
    private bool isAttack = true;
    private bool isCheck;


    public int damage;
    private float speed = 60f;
    private Vector3 startPosition;

    void Start()
    {
        player = GameObject.Find("Player");
        monsters.isStop = false;

        // 플레이어 위치에 따라 공격 방향 설정
        isLeftAttack = player.transform.position.x < monster.position.x;

        StartCoroutine(MonsterAttack());
    }

    private IEnumerator MonsterAttack()
    {
        while (true)
        {
            startPosition = monster.position;
            transform.position = startPosition;
            if (isCool)
            {
                isCool = false;
                if(player != null)
                {
                    isLeftAttack = player.transform.position.x < monster.position.x;
                }
               
                if (isLeftAttack)
                {
                    isCheck = true;
                    yield return StartCoroutine(AttackLeft());
                }
                else
                {
                    isCheck = false;
                    yield return StartCoroutine(AttackRight());
                }

                // 공격이 끝난 후 크기 줄이기
                yield return StartCoroutine(ReduceSize());

                StartCoroutine(TougueCool());
                isAttack = true;
                monsters.isStop = true;

                yield return null;
            }
           
            yield return null;
        }
        

    }

    private IEnumerator AttackLeft()
    {
        if(isAttack)
        {
            isAttack = false;
            while (transform.localScale.x < 10f)
            {
                transform.localScale = new Vector2(transform.localScale.x + (speed * Time.deltaTime), transform.localScale.y);
                transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime) * 2, transform.position.y);

                yield return null;
            }
        }
        
    }

    private IEnumerator AttackRight()
    {
        if (isAttack)
        {
            isAttack = false;
            while (transform.localScale.x < 10f)
            {
                transform.localScale = new Vector2(transform.localScale.x + (speed * Time.deltaTime), transform.localScale.y);
                transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime) * 2, transform.position.y);

                yield return null;
            }
        }
        
    }

    private IEnumerator ReduceSize()
    {
        if (isLeftAttack)
        {
            if (isCheck)
            {
                while (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x - (speed * Time.deltaTime), transform.localScale.y);
                    transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime) * 2, transform.position.y);

                    yield return null;
                }
            }
            
        }
        else
        {
            if (!isCheck)
            {
                while (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x - (speed * Time.deltaTime), transform.localScale.y);
                    transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime) * 2, transform.position.y);

                    yield return null;
                }
            }
            
        }
        
    }

    private IEnumerator TougueCool()
    {
        yield return new WaitForSeconds(3);
        isCool = true;
    }
}

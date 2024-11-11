//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerCheckMonster : MonoBehaviour
//{
//    public delegate bool PlayerDeath();
//    public event PlayerDeath deathPlayer;
//    private bool isDeath;

//    public bool isAttack = true;
//    public bool isEnemy = true;
//    private MovePlayer move;
//    private PlayerStatus status;
//    private CameraMove cam;

//    private Coroutine currentCor;
//    private void Start()
//    {
//        move = GetComponent<MovePlayer>();
//        status = GetComponent<PlayerStatus>();
//        cam = Camera.main.GetComponent<CameraMove>();
//    }
//    private void Update()
//    {
//        if ((deathPlayer?.Invoke() ?? false) && !isDeath)
//        {
//            isDeath = true;
//            Debug.Log("플레이어는 죽었습니다");
//        }
//    }

//    //적 관련 체크
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
//        {
//            isAttack = false;
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
//            move.enabled = false;
//            currentCor = StartCoroutine(WaitSecond());
//        }
//        else if (collision.gameObject.tag == "Tongue" && isAttack)
//        {
//            isAttack = false;
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster02_skilObj>().damage);
//            currentCor = StartCoroutine(WaitSecond());
//        }
//        else if (collision.gameObject.tag == "Boss" && isAttack)
//        {
//            isAttack = false;
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Boss>().damage);
//            currentCor = StartCoroutine(WaitSecond());
//        }
//        else if (collision.gameObject.tag == "BossSkil" && isAttack)
//        {
//            cam.Shake(0.2f, 0.5f, 0.2f);
//            if (collision.gameObject != null)
//            {
//                GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Boss>().damage);
//            }


//        }
//        else if (collision.gameObject.CompareTag("EnemySkil"))
//        {
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
//            move.enabled = false;
//            Destroy(collision.gameObject);
//            currentCor = StartCoroutine(WaitSecond());
//        }

//        //아이템
//        if (collision.gameObject.CompareTag("Item"))
//        {
//            Item item = collision.gameObject.GetComponent<Item>();
//            if (item != null)
//            {
//                ItemData itemData = item.GetItemData();
//                InventoryManager.Instance.AddItem(itemData);
//                Destroy(collision.gameObject); // 아이템 오브젝트 삭제
//            }
//        }




//    }
//    private void OnCollisionStay2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
//        {
//            isAttack = false;
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
//            currentCor = StartCoroutine(WaitSecond());
//        }

//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Boss") && isAttack)
//        {
//            isAttack = false;
//            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Boss>().damage);
//            currentCor = StartCoroutine(WaitSecond());
//        }

//        //보스 스킬
//        if (collision.CompareTag("BossSkil") && isAttack)
//        {

//            isAttack = false;
//            cam.Shake(0.35f, 1.2f, 0.4f);
//            GameManager.Instance.playerManager.PlayerTakeDamage(25);
//            currentCor = StartCoroutine(WaitSecond());
//        }
//    }

//    private IEnumerator WaitSecond()
//    {

//        yield return new WaitForSeconds(0.3f);
//        move.enabled = true;
//        yield return new WaitForSeconds(0.3f);
//        isAttack = true;

//        currentCor = null;
//    }
//}

using System.Collections;
using UnityEngine;

public class PlayerCheckMonster : MonoBehaviour
{
    public delegate bool PlayerDeath();
    public event PlayerDeath deathPlayer;
    private bool isDeath;

    public bool isAttack = true;
    public bool isEnemy = true;
    private MovePlayer move;
    private PlayerStatus status;
    private CameraMove cam;

    private Coroutine currentCor;

    private void Start()
    {
        move = GetComponent<MovePlayer>();
        status = GetComponent<PlayerStatus>();
        cam = Camera.main.GetComponent<CameraMove>();
    }

    private void Update()
    {
        if ((deathPlayer?.Invoke() ?? false) && !isDeath)
        {
            isDeath = true;
            Debug.Log("플레이어는 죽었습니다");
        }
    }

    // 중복된 코루틴 호출을 줄이는 메서드
    private void StartCoroutineWithCheck()
    {
        if (currentCor == null)
        {
            currentCor = StartCoroutine(WaitSecond());
        }
    }

    // 적 관련 체크
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
        {
            ProcessAttack(collision.gameObject.GetComponent<Monster>().damage);
        }
        else if (collision.gameObject.tag == "Tongue" && isAttack)
        {
            ProcessAttack(collision.gameObject.GetComponent<Monster02_skilObj>().damage);
        }
        else if (collision.gameObject.tag == "Boss" && isAttack)
        {
            ProcessAttack(collision.gameObject.GetComponent<Boss>().damage);
        }
        else if (collision.gameObject.tag == "BossSkil" && isAttack)
        {
            cam.Shake(0.2f, 0.5f, 0.2f);
            if (collision.gameObject != null)
            {
                ProcessAttack(collision.gameObject.GetComponent<Boss>().damage);
            }
        }
      

        // 아이템
        if (collision.gameObject.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                ItemData itemData = item.GetItemData();
                InventoryManager.Instance.AddItem(itemData);
                Destroy(collision.gameObject); // 아이템 오브젝트 삭제
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
        {
            ProcessAttack(collision.gameObject.GetComponent<Monster>().damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss") && isAttack)
        {
            ProcessAttack(collision.gameObject.GetComponent<Boss>().damage);
        }

        // 보스 스킬
        if (collision.CompareTag("BossSkil") && isAttack)
        {
            isAttack = false;
            cam.Shake(0.35f, 1.2f, 0.4f);
            GameManager.Instance.playerManager.PlayerTakeDamage(25);
            StartCoroutineWithCheck();
        }

        if (collision.CompareTag("EnemySkil"))
        {
            GameManager.Instance.playerManager.PlayerTakeDamage(20);
            move.enabled = false;
            Destroy(collision.gameObject);
            StartCoroutineWithCheck();
        }
    }

    // 공격 처리 로직을 함수로 분리하여 중복 제거
    private void ProcessAttack(int damage)
    {
        isAttack = false;
        GameManager.Instance.playerManager.PlayerTakeDamage(damage);
        move.enabled = false;
        StartCoroutineWithCheck();
    }

    private IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(0.3f);
        move.enabled = true;
        yield return new WaitForSeconds(0.3f);
        isAttack = true;

        currentCor = null;
    }
}



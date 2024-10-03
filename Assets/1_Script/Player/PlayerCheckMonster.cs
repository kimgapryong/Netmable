using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCheckMonster : MonoBehaviour
{
    public bool isAttack = true;
    public bool isEnemy = true;
    private MovePlayer move;
    private PlayerStatus status;

    private void Start()
    {
        move = GetComponent<MovePlayer>();  
        status = GetComponent<PlayerStatus>();
    }

    //�� ���� üũ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
            move.enabled = false;   
            StartCoroutine(WaitSecond());
        }
        else if (collision.gameObject.tag == "Tongue" && isAttack)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster02_skilObj>().damage);
            StartCoroutine(WaitSecond());
        }else if(collision.gameObject.tag == "Boss" && isAttack)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Boss>().damage);
            StartCoroutine(WaitSecond());
        }

        //������
        if (collision.gameObject.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                ItemData itemData = item.GetItemData();
                InventoryManager.Instance.AddItem(itemData);
                Destroy(collision.gameObject); // ������ ������Ʈ ����
            }
        }

      
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack && isEnemy)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
            StartCoroutine(WaitSecond());
        }
    }

    private IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(0.5f);
        move.enabled = true;
        yield return new WaitForSeconds(0.7f);
        isAttack = true;
    }



   
}


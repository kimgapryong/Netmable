using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCheckMonster : MonoBehaviour
{
    private bool isAttack = true;

    //적 관련 체크
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
            StartCoroutine(WaitSecond());
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttack)
        {
            isAttack = false;
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
            StartCoroutine(WaitSecond());
        }
    }

    private IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(1.5f);
        isAttack = true;
    }


    //아이템 관련 체크
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                ItemData itemData = item.GetItemData();
                InventoryManager.Instance.AddItem(itemData);
                Destroy(collision.gameObject); // 아이템 오브젝트 삭제
            }
        }
    }
}


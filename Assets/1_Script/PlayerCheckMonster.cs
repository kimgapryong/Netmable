using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCheckMonster : MonoBehaviour
{
    private bool isAttack = true;
    //�� ���� üũ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.playerManager.PlayerTakeDamage(collision.gameObject.GetComponent<Monster>().damage);
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
        yield return new WaitForSeconds(1);
        isAttack = true;
    }

    //������ ���� üũ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<Item>();
            item.count++;
            InventoryManager.Instance.AddItem(item);
            Destroy(collision.gameObject);
        }
    }
}

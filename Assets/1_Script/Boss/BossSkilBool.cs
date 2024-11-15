using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkilBool : MonoBehaviour
{
    private int damage = 20;
    private float time = 0f;
    private float setTime = 1.5f;
    private float speed = 300f;
    private bool isAttack = false;  
    public IEnumerator MoveBool(Vector2 vec)
    {
        while(time < setTime)
        {
            transform.Translate(vec * speed * Time.deltaTime);
            yield return null;
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isAttack)
            {
                isAttack = true;
                PlayerManager.Instance.PlayerTakeDamage(damage);
            }
        }
    }
}

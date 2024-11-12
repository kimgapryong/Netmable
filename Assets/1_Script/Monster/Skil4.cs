using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skil4 : Skil
{

    public SkilData data;
    private void Start()
    {
        ResetSkil(data);
        StartCoroutine(waitSkil());
    }
    public IEnumerator waitSkil()
    {
        while (status == null)
        {
            yield return null;
        }
        status.OnLevelUp += SkilConditon;
    }

    public void SkilConditon()
    {
        Debug.Log("Current Level: " + status.currentLevel + ", Skill Level: " + SkilLevel);
        if (status.currentLevel >= SkilLevel)
        {
            playerSkils.LengendCheck(key, skilPrefab);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("d왜 아노대");
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Monster>().normalAttack = false;
            collision.gameObject.GetComponent<Monster>().TakeDamage(status.damage + damage);
        }
        else if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().TakeDamage(status.damage + damage / 5);
        }
    }
    public bool isatk = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isatk)
        {
            isatk = false;
            if (collision.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Monster>().normalAttack = false;
                collision.gameObject.GetComponent<Monster>().TakeDamage((status.damage + damage )/ 10);
            }
            else if (collision.CompareTag("Boss"))
            {
                collision.gameObject.GetComponent<Boss>().TakeDamage((status.damage + damage) / 15);
            }
            StartCoroutine(WaitSkil());
        }
        
    }

    private IEnumerator WaitSkil()
    {
        yield return new WaitForSeconds(0.2f);
        isatk = true;
    }
}

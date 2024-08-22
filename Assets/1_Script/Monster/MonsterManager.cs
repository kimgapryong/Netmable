using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private Transform player;
    public List<Monster> monsters = new List<Monster>();

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        monsters.AddRange(FindObjectsOfType<Monster>());
    }

    private void Update()
    {
        isAttackTrue();
    }
    
    private void isAttackTrue()
    {
        foreach (Monster monster in monsters)
        {
            if (monster != null && player != null)
            {
                if (Vector2.Distance(player.position, monster.gameObject.transform.position) < 35f)
                {
                    monster.isAttack = true;
                }
                else
                {
                    monster.isAttack = false;
                }
            }
        }
    }


}
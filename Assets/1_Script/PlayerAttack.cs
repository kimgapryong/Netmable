using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float attackCooldown = 0.5f;

    private float lastAttackTime; 

    private void Start()
    {
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -90));
        lastAttackTime = Time.time;
    }
}

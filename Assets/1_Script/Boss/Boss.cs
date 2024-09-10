using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{

    public string bossName;

    public float maxHp;
    public float currentHp;

    public float damage;

    public Animator animators;
    public void TakeDamage(float dam)
    {
        currentHp -= dam;
    }
    
    public IEnumerator IdleBoss(float time)
    {
        transform.position = Vector3.zero;
        yield return new WaitForSeconds(time);
    }
}

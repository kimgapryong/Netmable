using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public delegate void PlayerTrigger(int damage);
    public event PlayerTrigger OnPlayerTrigger;

    public string bossName;
    public CameraMove cam;

    public int maxHp;
    public int currentHp;

    public int damage;

    public Animator animators;

    public void GetBossData(string bossName, int maxHp, int damage)
    {
        this.bossName = bossName;
        this.maxHp = maxHp;
        this.damage = damage;
    }
    public IEnumerator GetCam()
    {
        yield return new WaitForSeconds(1);
        GameObject cameraObj = GameObject.Find("Main Camera"); 
        if (cameraObj != null)
        {
            cam = cameraObj.GetComponent<CameraMove>(); 
            Debug.Log(cam.name);
        }
    }
    public void TakeDamage(int dam)
    {
        currentHp -= dam;
    }
    public void PlayerDamageTirgger()
    {
        Debug.Log(1);
        OnPlayerTrigger?.Invoke(damage);
    }
    public IEnumerator IdleBoss(float time)
    {
        transform.position = Vector3.zero;
        yield return new WaitForSeconds(time);
    }
}

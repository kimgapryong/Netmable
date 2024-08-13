using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class PlayerSkils : MonoBehaviour
{
    //불 스킬 레벨 확인
    public bool FireSkil = false;
    //불 스킬 쿨타임
    private bool FireCoolTimes = true;
    public MovePlayer movePlayer;

    //스킬 오브젝트
    private GameObject fireObj;

    //스킬 키코드
    private KeyCode fireKey;

    //스킬 넣어주는 액션
    public FireBoolSkil fireBool;


    private void Update()
    {
        FireBoolSkil();
    }
    private void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        StartCoroutine(fireBool.waitSkil()); //파이버블 Action
    }
     public void FireCheck(KeyCode key,GameObject fire)
    {
        fireKey = key;
        fireObj = fire;
        
    }
    public void FireBoolSkil()
    {
        if(FireSkil)
        {
            if (UnityEngine.Input.GetKeyDown(fireKey) && FireCoolTimes)
            {
                FireCoolTimes = false;
                if (movePlayer.facingRight)
                {
                    Instantiate(fireObj, transform.position, Quaternion.identity);
                }
                else if (!movePlayer.facingRight)
                {
                    Instantiate(fireObj, transform.position, Quaternion.Euler(0, 0, 180));
                }
                StartCoroutine(FireCoolTime());
            }

        }
    }
    private IEnumerator FireCoolTime()
    {
        yield return new WaitForSeconds(10f);
        FireCoolTimes = true ;
    }

}

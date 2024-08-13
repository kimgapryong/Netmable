using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using UnityEngine;

public class PlayerSkils : MonoBehaviour
{
    //�� ��ų ���� Ȯ��
    public bool FireSkil = false;
    //�� ��ų ��Ÿ��
    private bool FireCoolTimes = true;
    public MovePlayer movePlayer;

    //��ų ������Ʈ
    private GameObject fireObj;

    //��ų Ű�ڵ�
    private KeyCode fireKey;

    //��ų �־��ִ� �׼�
    public FireBoolSkil fireBool;


    private void Update()
    {
        FireBoolSkil();
    }
    private void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
        StartCoroutine(fireBool.waitSkil()); //���̹��� Action
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

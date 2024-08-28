using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSkils : MonoBehaviour
{
    //�� ��ų ���� Ȯ��
    public bool FireSkil = false;
    public bool isChaging = false;
    //�� ��ų ��Ÿ��
    private bool FireCoolTimes = true;
    private bool ChaCoolTimes = true;
    public MovePlayer movePlayer;
    public Transform SkilsFire;

    //��ų ������Ʈ
    private GameObject fireObj;
    //��¡
    private GameObject chaObj;


    //��ų Ű�ڵ�
    private KeyCode fireKey;
    private KeyCode chagingkey;

    //��ų �־��ִ� �׼�
    public FireBoolSkil fireBool;



    private void Update()
    {
        FireBoolSkil();
        UseChaSkil();
    }
    private void Start()
    {
        movePlayer = GameObject.Find("Player").GetComponent<MovePlayer>();
       
        StartCoroutine(fireBool.waitSkil()); //���̹��� Action
    }

    
    //���̾� ��ų �޼���
     public void FireCheck(KeyCode key,GameObject fire)
    {
        fireKey = key;
        fireObj = fire;
        
    }
    public void FireBoolSkil()
    {
        if(FireSkil)
        {
            if (Input.GetKeyDown(fireKey) && FireCoolTimes)
            {
                FireCoolTimes = false;
                if (movePlayer.facingRight)
                {
                    Instantiate(fireObj, SkilsFire.position, Quaternion.identity);
                }
                else if (!movePlayer.facingRight)
                {
                    Instantiate(fireObj, SkilsFire.position, Quaternion.Euler(0, 0, 180));
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

    //��¡�� 
    public void ChagingCheck(KeyCode key ,GameObject cha)
    {
        chagingkey = key;
        chaObj = cha;
 
    }

    public void UseChaSkil()
    {
        if (isChaging && ChaCoolTimes)
        {
 
            if (Input.GetKeyDown(chagingkey))
            {
                ChaCoolTimes = false;
                GameObject currentSkillObject = Instantiate(chaObj, SkilsFire.position, Quaternion.identity);
                ChagingSkil chagingSkilComponent = currentSkillObject.GetComponent<ChagingSkil>();

                StartCoroutine(ChargeSkill(currentSkillObject, chagingSkilComponent));
            }
        }
    }

    public int chaDamage = 0;
    public int GetChaDamage()
    {
        return chaDamage;
    }
    private IEnumerator ChargeSkill(GameObject currentSkillObject, ChagingSkil chaSkil)
    {
        float chargingTime = 0f;
        int maxDamage = 100;
        if (currentSkillObject != null)
        {
            while (Input.GetKey(chagingkey))
            {
                chargingTime += Time.deltaTime;

                if (chargingTime <= 4f)
                {
                    float scaleFactor = Mathf.Lerp(1f, 3.5f, chargingTime / 4f);
                    currentSkillObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                    chaDamage = Mathf.RoundToInt(Mathf.Lerp(0, maxDamage, chargingTime / 4));
                }
                else if (chargingTime >= 8f)
                {
                    Destroy(currentSkillObject);
                    StartCoroutine(ChaCoolTime());
                    yield break;
                }

                if (currentSkillObject != null)
                {
                    currentSkillObject.transform.position = SkilsFire.position;
                }

                yield return null;
            }
        
            if (currentSkillObject != null)
            {
                chaSkil.collider2D.enabled = true;
                if (movePlayer.facingRight)
                {
                 
                    Destroy(currentSkillObject, 2.3f);
                }
                else
                {
                    currentSkillObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            
                    Destroy(currentSkillObject, 2.3f);
                }
            }

            StartCoroutine(ChaCoolTime());
        }
       
    }



    private IEnumerator ChaCoolTime()
    {
    
        yield return new WaitForSeconds(1f);
        ChaCoolTimes = true;
    }
}

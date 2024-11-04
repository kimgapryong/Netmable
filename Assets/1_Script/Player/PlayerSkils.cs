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
    private bool shieldTimes = true;
    public MovePlayer movePlayer;
    public PlayerStatus status;
    public Transform SkilsFire;

    private UiManager uiManager;

    //��ų ������Ʈ
    private GameObject fireObj;
    //��¡
    private GameObject chaObj;

    private int fireMana = 30;
    private float chaMana;
    private int shieldMana = 15;

    //��ų Ű�ڵ�
    private KeyCode fireKey;
    private KeyCode chagingkey;
    private KeyCode shieldKey;

    //��ų �־��ִ� �׼�
    public FireBoolSkil fireBool;



    private void Update()
    {
        //FireBoolSkil();
        //UseChaSkil();
        //ShieldSkil();
    }
    private void Start()
    {
        movePlayer = GetComponent<MovePlayer>();
        status = GetComponent<PlayerStatus>();
        uiManager = UiManager.Instance; 
        StartCoroutine(fireBool.waitSkil()); //���̹��� Action
    }

    
    //���̾� ��ų �޼���
     public void FireCheck(KeyCode key,GameObject fire)
    {
        fireKey = key;
        fireObj = fire;
        uiManager.skil1.SetActive(true);
    }
    public void FireBoolSkil()
    {
        if(FireSkil)
        {
            if (FireCoolTimes && status.ManaOk(fireMana))
            {
                FireCoolTimes = false;
                status.currentMp -= fireMana;
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
        uiManager.skil2.SetActive(true);
    }
    private bool chaTrue = false;
    public void UseChaSkil()
    {
        if (isChaging && ChaCoolTimes)
        {
 
            //if (Input.GetKeyDown(chagingkey))
            
                ChaCoolTimes = false;
                GameObject currentSkillObject = Instantiate(chaObj, SkilsFire.position, Quaternion.identity);
                ChagingSkil chagingSkilComponent = currentSkillObject.GetComponent<ChagingSkil>();
                chaTrue = true;
                StartCoroutine(ChargeSkill(currentSkillObject, chagingSkilComponent));
            
        }
    }
    public void DelChaSkil()
    {
        chaTrue = false;
    }

    public int chaDamage = 0;

    public int GetChaDamage()
    {
        return chaDamage;
    }
    public IEnumerator ChargeSkill(GameObject currentSkillObject, ChagingSkil chaSkil)
    {
        float chargingTime = 0f;
        int maxDamage = 100;
        if (currentSkillObject != null)
        {
            while (chaTrue && status.ManaOk((int)chaMana))
            {
                chargingTime += Time.deltaTime;

                if (chargingTime <= 4f)
                {
                    chaMana += 0.02f;
                 
                    float scaleFactor = Mathf.Lerp(1f, 3.5f, chargingTime / 4f);
                    currentSkillObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                    chaDamage = Mathf.RoundToInt(Mathf.Lerp(0, maxDamage, chargingTime / 4));
                }
                else if (chaTrue && chargingTime >= 8f)
                {
                    status.currentMp -= (int)chaMana;
                    chaMana = 0;
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
                status.currentMp -= (int)chaMana;
                chaMana = 0;
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

    //����
    public void GetShield(KeyCode key)
    {
        Debug.Log("���� ���");
        shieldKey = key;
        uiManager.skil3.SetActive(true);
    }
    public void ShieldSkil()
    {
        if(shieldTimes && status.ManaOk(shieldMana))
        {
            Debug.Log("���彺ų");
            shieldTimes = false;
            status.currentMp -= shieldMana;
            PlayerManager.Instance.okAtk = false;
            StartCoroutine(ShieldCool());
        }
    }
    private IEnumerator ShieldCool()
    {
        yield return new WaitForSeconds(3f);
        PlayerManager.Instance.okAtk = true;
        yield return new WaitForSeconds(1f);
        shieldTimes = true;
    }
}


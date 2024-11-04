using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSkils : MonoBehaviour
{
    //불 스킬 레벨 확인
    public bool FireSkil = false;
    public bool isChaging = false;
    //불 스킬 쿨타임
    private bool FireCoolTimes = true;
    private bool ChaCoolTimes = true;
    private bool shieldTimes = true;
    public MovePlayer movePlayer;
    public PlayerStatus status;
    public Transform SkilsFire;

    private UiManager uiManager;

    //스킬 오브젝트
    private GameObject fireObj;
    //차징
    private GameObject chaObj;

    private int fireMana = 30;
    private float chaMana;
    private int shieldMana = 15;

    //스킬 키코드
    private KeyCode fireKey;
    private KeyCode chagingkey;
    private KeyCode shieldKey;

    //스킬 넣어주는 액션
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
        StartCoroutine(fireBool.waitSkil()); //파이버블 Action
    }

    
    //파이어 스킬 메서드
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

    //차징볼 
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

    //쉴드
    public void GetShield(KeyCode key)
    {
        Debug.Log("쉴드 등록");
        shieldKey = key;
        uiManager.skil3.SetActive(true);
    }
    public void ShieldSkil()
    {
        if(shieldTimes && status.ManaOk(shieldMana))
        {
            Debug.Log("쉴드스킬");
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


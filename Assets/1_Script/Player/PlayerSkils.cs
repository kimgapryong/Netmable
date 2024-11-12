using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Animations;

public class PlayerSkils : MonoBehaviour
{
    //�� ��ų ���� Ȯ��
    public bool FireSkil = false;
    public bool isChaging = false;
    //�� ��ų ��Ÿ��
    public Animator animator;
    private bool FireCoolTimes = true;
    private bool ChaCoolTimes = true;
    private bool shieldTimes = true;
    private bool legendTimes = true;
    public MovePlayer movePlayer;
    public PlayerStatus status;
    public Transform SkilsFire;

    private UiManager uiManager;

    //��ų ������Ʈ
    private GameObject fireObj;
    //��¡
    private GameObject chaObj;
    //�ʻ��
    private GameObject legendObj;

    private int fireMana = 30;
    private float chaMana;
    private int shieldMana = 15;
    private int legendMana = 20;

    //��ų Ű�ڵ�
    private KeyCode fireKey;
    private KeyCode chagingkey;
    private KeyCode shieldKey;
    private KeyCode legendKey;

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
        animator = GetComponent<Animator>();
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

    public void LengendCheck(KeyCode key,GameObject fire)
    {
        legendKey = key;
        legendObj = fire;
        uiManager.skil4.SetActive(true);
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
        status.currentMp -= (int)chaMana;
        chaMana = 0;
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
                    chaMana += 0.03f;
                 
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

    public void LegendSKil()
    {
        if(legendTimes && status.ManaOk(legendMana))
        {
            legendTimes = false;
            animator.SetBool("Skil4", true);
            status.currentMp -= legendMana;
            StartCoroutine(Skil4Legend());
        }
    }
    private List<GameObject> LenList = new List<GameObject>();
    private IEnumerator Skil4Legend()
    {
        float xVec = 8f;
        float yVex = 3f;
        yield return StartCoroutine(GetSkil4Anima());
        animator.SetBool("Skil4", false);
        if (movePlayer.facingRight)
        {
            for (int i = 1; i <= 6; i++)
            {
                GameObject clone = Instantiate(legendObj, new Vector2(transform.position.x + xVec * i, transform.position.y + yVex), Quaternion.identity);
                clone.GetComponent<Animator>().SetTrigger("FireCur");
                LenList.Add(clone);
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (!movePlayer.facingRight)
        {
            for (int i = 1; i <= 6; i++)
            {
                GameObject clone = Instantiate(legendObj, new Vector2(transform.position.x - xVec * i, transform.position.y + yVex), Quaternion.identity);
                clone.GetComponent<Animator>().SetTrigger("FireCur");
                LenList.Add(clone);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return new WaitForSeconds(5f);
        for(int i =0; i < LenList.Count; i++)
        {
            LenList[i].GetComponent<Animator>().SetTrigger("FireLow");
            yield return new WaitForSeconds(0.3f);
            GameObject go = LenList[i];
            LenList.Remove(go);
            Destroy(go);
            
        }
        StartCoroutine(LenCool());
    }

    private IEnumerator LenCool()
    {
        yield return new WaitForSeconds(1f);
        legendTimes = true;
    }

    private IEnumerator GetSkil4Anima()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (animatorStateInfo.normalizedTime < 1.0f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; // �� �����Ӹ��� ���
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkils : MonoBehaviour
{
    //불 스킬 레벨 확인
    public bool FireSkil = false;
    public bool isChaging = false;
    //불 스킬 쿨타임
    public Animator animator;
    private bool FireCoolTimes = true;
    private bool ChaCoolTimes = true;
    private bool shieldTimes = true;
    private bool legendTimes = true;
    public MovePlayer movePlayer;
    public PlayerStatus status;
    public Transform SkilsFire;
    private CameraMove cam;

    private UiManager uiManager;

    //스킬 오브젝트
    private GameObject fireObj;
    //차징
    private GameObject chaObj;
    //필살기
    private GameObject legendObj;

    private int fireMana = 30;
    private float chaMana;
    private int shieldMana = 15;
    private int legendMana = 20;

    //스킬 키코드
    private KeyCode fireKey;
    private KeyCode chagingkey;
    private KeyCode shieldKey;
    private KeyCode legendKey;

    //스킬 넣어주는 액션
    public FireBoolSkil fireBool;

    public AudioClip skil2Clip;
    public AudioClip skil4Clip;


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
        cam = Camera.main.GetComponent<CameraMove>();
        StartCoroutine(fireBool.waitSkil()); //파이버블 Action
    }

    
    //파이어 스킬 메서드
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
                animator.SetBool("isFire", true);
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
        StartCoroutine(GetSkil3Anima());
        yield return StartCoroutine(GetSkil3Anima());
        animator.SetBool("isFire", false);
        yield return new WaitForSeconds(10f);
        FireCoolTimes = true ;
    }

    //차징볼 
    public Sprite cha1;
    public Sprite cha2;
    public Sprite cha3;
    public Sprite cha4;
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
            SoundManager.Instance.SFXSound("Chaging", skil2Clip);
            GameObject currentSkillObject = Instantiate(chaObj, SkilsFire.position, Quaternion.identity);
            currentSkillObject.GetComponent<SpriteRenderer>().sprite = cha1;
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
                    
                    if(currentSkillObject.transform.localScale.x >= 3.4f)
                    {
                        currentSkillObject.GetComponent<SpriteRenderer>().sprite = cha4;
                    }
                    else if (currentSkillObject.transform.localScale.x >= 3f)
                    {
                        currentSkillObject.GetComponent<SpriteRenderer>().sprite = cha3;
                    }
                    else if (currentSkillObject.transform.localScale.x >= 2f)
                    {
                        currentSkillObject.GetComponent<SpriteRenderer>().sprite = cha2;
                    }

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
              
                chaSkil.colliderObj.enabled = true;
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
            animator.SetBool("isShiled", true);
            uiManager.defence.SetActive(true);
            shieldTimes = false;
            status.currentMp -= shieldMana;
            PlayerManager.Instance.okAtk = false;

            StartCoroutine(ShieldCool());
        }
    }
    private IEnumerator ShieldCool()
    {
        yield return StartCoroutine(GetSkil3Anima());
        animator.SetBool("isShiled", false);
        yield return new WaitForSeconds(3f);
        uiManager.defence.SetActive(false);
        PlayerManager.Instance.okAtk = true;
        yield return new WaitForSeconds(1f);
        shieldTimes = true;
    }

    public void LegendSKil()
    {
        if(legendTimes && status.ManaOk(legendMana))
        {
            legendTimes = false;
            SoundManager.Instance.SFXSound("Legned", skil4Clip);
            animator.SetBool("Skil4", true);
            status.currentMp -= legendMana;
            StartCoroutine(Skil4Legend());
        }
    }
    public List<GameObject> LenList = new List<GameObject>();
    private List<GameObject> objectsToRemove = new List<GameObject>();
    private IEnumerator Skil4Legend()
    {

        float xVec = 8f;
        float yVex = 3f;
        yield return new WaitForSeconds(skil4Clip.length - 1.5f);
        animator.SetBool("Skil4", false);
        if (movePlayer.facingRight)
        {
            for (int i = 1; i <= 6; i++)
            {
                GameObject clone = Instantiate(legendObj, new Vector2(transform.position.x + xVec * i, transform.position.y + yVex), Quaternion.identity);
                cam.player = clone.transform;
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
                cam.player = clone.transform;
                clone.GetComponent<Animator>().SetTrigger("FireCur");
                LenList.Add(clone);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        cam.player = transform;
        yield return new WaitForSeconds(5f);
        foreach (var go in LenList)
        {
            go.GetComponent<Animator>().SetTrigger("FireLow");
            yield return new WaitForSeconds(0.3f);
            objectsToRemove.Add(go);  
            Destroy(go);
        }

        // 삭제할 요소들을 한 번에 삭제
        foreach (var go in objectsToRemove)
        {
            LenList.Remove(go);
        }
        StartCoroutine(LenCool());
    }

    private IEnumerator LenCool()
    {
        yield return new WaitForSeconds(1f);
        legendTimes = true;
    }

    private IEnumerator GetSkil3Anima()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        movePlayer.canMove = false;
        while (animatorStateInfo.normalizedTime < 1.0f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; // 매 프레임마다 대기
        }
        movePlayer.canMove = true;
    }
}


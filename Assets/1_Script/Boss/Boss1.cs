
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class Boss1 : Boss
{
    public BossScriptable data;
    public bool isMove = true;
    public float layerSpeed = 60f;
   
    public GameObject skill1obj;
    public GameObject skill2obj;
    public GameObject magicHole;

    public Transform grounds;
    public Transform center;
    public Transform[] attackTrans;
    public List<Vector2> electryTrans;

    public GameObject partic;
    public ParticleSystem particle;

    public GameObject fire;
    public GameObject normal1;
    public GameObject normal2;

    protected override void Start()
    {
        GetBossData(data.bossName, data.maxHp, data.damage, data.speed);
        base.Start();
        find = GetComponent<Finding>();
        StartCoroutine(GetCam());
        skilCount = 3;

        find.normal += NormalAttack1;
        find.normal += NormalAttack2;

        //질문 유니티상에 bossUI의 헬스바 슬라이더 설정하는 거
        //질문 코루틴을 반복문으로 리스트에 넣는 법 물어보기
        bossSkils.Add(Attack1);
        bossSkils.Add(Attack3);
       
    }
    protected override void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(Attack1());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(damage);
            StartCoroutine(Attack2());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(damage);
            StartCoroutine(Attack3());
        }

        if (isIdle && !attackCool)
        {
            find.isWalk = false;
            animator.SetBool("Side", false);
            fsm.ChangeState(State.Idle);
            StartCoroutine(StateIdle());
            
        }else if(isAttack)
        {

            fsm.ChangeState(State.Attack);
            StartCoroutine(StateAttack());

        }
        base.LateUpdate();
    }
    public override IEnumerator Attack1()
    {
        attackCool = true;
        StartCoroutine(ui.Attack1Ui());
        yield return StartCoroutine(ui.Attack1Ui());
        yield return new WaitForSeconds(0.4f);
        for(int i = 0; i < ui.attack1Image.Length; i++)
        {
            GameObject clone = Instantiate(skill1obj, attackTrans[i].position, Quaternion.identity);
            BossSkilBool bossSkil = clone.GetComponent<BossSkilBool>();
            switch (i)
            {
                
                case 0:
                    StartCoroutine(bossSkil.MoveBool(Vector2.left)); break;
                case 1:
                    StartCoroutine(bossSkil.MoveBool(Vector2.down)); break;
                case 2:
                    StartCoroutine(bossSkil.MoveBool(Vector2.right)); break;
                case 3:
                    StartCoroutine(bossSkil.MoveBool(Vector2.up)); break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        attackCool = false;
    }

    public override IEnumerator Attack2()
    {
        attackCool = true;
        StartCoroutine (ParticleGet());
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ParticleGet()
    {
        float number = 90;
        float num = 30f;

        float radialNum = 0;
        float speedModifierNum = 0;
       
        GameObject clone = Instantiate(partic, center.position, Quaternion.identity);

        ParticleSystem particleInstance = clone.GetComponent<ParticleSystem>();
        var shapeModule = particleInstance.shape;
        var velocityModule = particleInstance.velocityOverLifetime;
        var eml = particleInstance.emission;
        velocityModule.enabled = false;

        velocityModule.radial = new ParticleSystem.MinMaxCurve(0);
        velocityModule.speedModifier = new ParticleSystem.MinMaxCurve(0);

        rb.bodyType = RigidbodyType2D.Static;
        isMove = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = center.position;
        
        col.isTrigger = true;
        //rb.constraints = (RigidbodyConstraints2D)(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);

        while (shapeModule.radius <= number)
        {
            shapeModule.radius += 20f;
            yield return null; 

            if (shapeModule.radius >= number)
            {
                yield return new WaitForSeconds(0.3f);
                velocityModule.enabled = true;
                eml.rateOverTime = 370;
                velocityModule.radial = new ParticleSystem.MinMaxCurve(radialNum);
                velocityModule.speedModifier = new ParticleSystem.MinMaxCurve(speedModifierNum);

      
                while (num > 0)
                {
                    
                    shapeModule.radius -= 2f;
                    radialNum += 0.7f;
                    velocityModule.radial = new ParticleSystem.MinMaxCurve(radialNum);
                    speedModifierNum -= 0.7f;
                    velocityModule.speedModifier = new ParticleSystem.MinMaxCurve(speedModifierNum);

                    yield return new WaitForSeconds(0.1f);
                    num--;
                  
                    if(num <= 0)
                    {
                        yield return new WaitForSeconds(2.2f);
                        eml.rateOverTime = 0;
                        yield return new WaitForSeconds(0.3f);
                        StartCoroutine(PaeticelEmisson(clone, velocityModule, eml));
                    }
                }

                break;
            }
        }
    }

    private IEnumerator PaeticelEmisson(GameObject clone, ParticleSystem.VelocityOverLifetimeModule life, ParticleSystem.EmissionModule emission)
    {
        int rate = 750;
        emission.rateOverTime = rate;
        life.radial = new ParticleSystem.MinMaxCurve(40);
        life.speedModifier = new ParticleSystem.MinMaxCurve(10);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(cam.Shake(5f, 1.4f, 0.4f));
        PlayerDamageTirgger();
        while (rate > 0)
        {
            rate -= Random.Range(3, 7);

            if(rate <= 0)
            {
                yield return new WaitForSeconds(4);
                col.isTrigger = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.None;
                //rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.FreezeRotationZ;
                Destroy(clone); 
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        find.enabled = true;
        attackCool = false;
        isMove = true;
        
    }

    public override IEnumerator Attack3()
    {
        attackCool = true;
        Vector2 elePos = new Vector2(0, -4.5f);
        int xPos = 15;
        
        electryTrans = new List<Vector2>();
        List<GameObject> hole = new List<GameObject>();
        if(electryTrans.Count <= 0)
        {
            electryTrans.Add(elePos);
            for (int i = 1; i <= 10; i++)
            {
                electryTrans.Add(new Vector2(elePos.x + (i * xPos), elePos.y));

                electryTrans.Add(new Vector2(elePos.x - (i * xPos), elePos.y));
            }
        }
        for (int i = electryTrans.Count - 1; i >= 0; i--)
        {
            GameObject clone = Instantiate(magicHole, electryTrans[i], Quaternion.identity);
            hole.Add(clone);
        }

        yield return new WaitForSeconds(1.5f);  
        foreach (GameObject obj in hole)
        {
            Destroy(obj);
        }
        StartCoroutine(ElectryAttack());
    }
    private IEnumerator ElectryAttack()
    {
        List<GameObject> clons = new List<GameObject>();
        float targetScaleY = 300f;  
        float scaleSpeed = 1000f;  

        for (int i = electryTrans.Count - 1; i >= 0; i--)
        {
            GameObject clone = Instantiate(skill2obj, electryTrans[i], Quaternion.identity);
            clons.Add(clone);
        }

        bool allScaled = false;

        while (!allScaled)
        {
            allScaled = true;

            List<GameObject> objectsToRemove = new List<GameObject>();

            foreach (GameObject clone in clons)
            {
                Vector3 currentScale = clone.transform.localScale;
                Vector3 currentPosition = clone.transform.position;

                if (currentScale.y < targetScaleY)
                {
   
                    float newScaleY = currentScale.y + scaleSpeed * Time.deltaTime;
                    clone.transform.localScale = new Vector3(currentScale.x, newScaleY, currentScale.z);

         
                    float heightDifference = (newScaleY - currentScale.y) / 2;  
                    clone.transform.position = new Vector3(currentPosition.x, currentPosition.y + heightDifference, currentPosition.z);

                    allScaled = false;  
                }
                else
                {
                    objectsToRemove.Add(clone);  
                }
            }

            foreach (GameObject obj in objectsToRemove)
            {
                clons.Remove(obj);
                Destroy(obj);
            }

            yield return null;
        }
        yield return new WaitForSeconds(1f);
        attackCool = false;
    }

    public void NormalAttack1()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 14 && isAttack)
        {
            GameObject clone = Instantiate(normal1, transform.position, Quaternion.identity);
            if(transform.localScale.x > 0)
            {
                clone.GetComponent<ScaleUp>().AttackRight(transform);
            }
            else if(transform.localScale.x < 0)
            {
                clone.GetComponent<ScaleUp>().AttackRight(transform);
            }
            
            //float bossScale = transform.localScale.x / 3;
            StartCoroutine(attackTime(transform.localScale.x / 3, clone));
            StartCoroutine(NormalCool());
        }
           
    }


    private IEnumerator attackTime(float bossScale, GameObject obj)
    {
        float speed = 45;
        float time = 0;
        float die = 3;
        Vector2 newVec = new Vector2(bossScale, 0);
        while (time <= die)
        {
            yield return null;
            if(obj != null)
            {
                obj.transform.Translate(newVec * speed * Time.deltaTime);
            }
            time += Time.deltaTime;
        }
    }

    public void NormalAttack2()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 30 && isAttack)
        {
            Vector2 newVec = new Vector2(player.transform.position.x, player.transform.position.y + 20);
            GameObject clone = Instantiate(normal2, newVec, Quaternion.identity);

            StartCoroutine(attackLength(player.transform.position, clone));
            StartCoroutine(NormalCool());
        }
        
    }

    private IEnumerator attackLength(Vector2 targetPosition, GameObject clone)
    {
        Vector3 startPosition = clone.transform.position;

        float lightningSpeed = 5f;  
        float targetScaleY = Vector2.Distance(startPosition, targetPosition);

        Destroy(clone, 0.8f);
        while (clone != null && clone.transform.localScale.y < targetScaleY)
        {
            clone.transform.localScale += new Vector3(0, lightningSpeed * Time.deltaTime, 0);
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, targetPosition, lightningSpeed * Time.deltaTime);

            yield return null;
        }


        //질문 보스스테이지 끝쪽을 보면 총알이 안 지워짐


    }

    private IEnumerator NormalCool()
    {
        isAttack = false;
        yield return new WaitForSeconds(3.5f);
        isAttack = true;
    }

    //boss Idle
    private float idleCool = 5f;
    private float attackState = 20f;
    public override void BossIdle()
    {

        //기달리는 애니메이션
        transform.position = new Vector2(2, -3.6f);
        StartCoroutine(StateIdle());
     
    }
    public override IEnumerator StateIdle()
    {
        find.enabled = false;
        yield return new WaitForSeconds(idleCool);
        find.isWalk = true;
        isIdle = false;
        find.enabled = true;
        isAttack = true;
        
    }
    public override IEnumerator StateAttack()
    {
        yield return new WaitForSeconds(attackState);
        isAttack = false;
        isIdle = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class Boss1 : Boss
{
    public Boss1Ui ui;
    public float speed = 10f;

    private GameObject player;
    private MovePlayer movePlayer;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isAttack = true;
    private bool isMove = true;
    private bool isGround;
    public Animator animator;

    public GameObject skill1obj;
    public GameObject skill2obj;
    public GameObject magicHole;

    public LayerMask ground;
    public Transform grounds;
    public Transform center;
    public Transform[] attackTrans;
    public List<Vector2> electryTrans;

    public GameObject partic;
    public ParticleSystem particle;

    private void Start()
    {
        player = GameObject.Find("Player");
        movePlayer = player.GetComponent<MovePlayer>();
        rb =GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        StartCoroutine(GetCam());
    }
    private void Update()
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
        BossMovers();
    }
    public IEnumerator Attack1()
    {
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
    }

    public IEnumerator Attack2()
    {
        StartCoroutine (ParticleGet());
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ParticleGet()
    {
        float number = 110;
        float num = 135f;

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

        transform.position = center.position;
        isMove = false;
        col.isTrigger = true;
        rb.constraints = (RigidbodyConstraints2D)(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);

        while (shapeModule.radius <= number)
        {
            shapeModule.radius += 0.3f;
            yield return null; 

            if (shapeModule.radius >= number)
            {
                yield return new WaitForSeconds(2.2f);
                velocityModule.enabled = true;
                eml.rateOverTime = 370;
                velocityModule.radial = new ParticleSystem.MinMaxCurve(radialNum);
                velocityModule.speedModifier = new ParticleSystem.MinMaxCurve(speedModifierNum);

      
                while (num > 0)
                {
                    
                    shapeModule.radius -= 0.6f;
                    radialNum += 0.05f;
                    velocityModule.radial = new ParticleSystem.MinMaxCurve(radialNum);
                    speedModifierNum -= 0.05f;
                    velocityModule.speedModifier = new ParticleSystem.MinMaxCurve(speedModifierNum);

                    yield return new WaitForSeconds(0.1f);
                    num--;
                  
                    if(num <= 0)
                    {
                        yield return new WaitForSeconds(1.5f);
                        eml.rateOverTime = 0;
                        yield return new WaitForSeconds(2f);
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
        StartCoroutine(cam.Shake(2f, 1.4f, 0.4f));
        PlayerDamageTirgger();
        while (rate > 0)
        {
            rate -= Random.Range(3, 7);

            if(rate <= 0)
            {
                yield return new WaitForSeconds(4);
                col.isTrigger = false;
                rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.None;
                rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.FreezeRotationZ;
                Destroy(clone); 
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        isMove = true;
    }

    public IEnumerator Attack3()
    {
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
    }

    public override void BossMovers()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);


        if (distanceToPlayer > 3f)
        {
            StartCoroutine(FollowPlayer());
        }
    }

    private IEnumerator FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (isMove)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }


        if (player.transform.position.y > transform.position.y + 3 && movePlayer.isGround)
        {
            isMove = false;


            if (movePlayer.facingRight)
            {
                transform.position = new Vector3(player.transform.position.x - 1.5f, transform.position.y + 3, 0);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + 1.5f, transform.position.y + 3, 0);
            }

            yield return new WaitForSeconds(0.4f);  


            if (!IsGrounded())
            {

                if (movePlayer.facingRight)
                {
                    transform.position = new Vector3(player.transform.position.x + 2f, transform.position.y + 3, 0);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x - 2f, transform.position.y + 3, 0);
                }
            }

            yield return new WaitForSeconds(0.6f); 

            if (IsGrounded())
            {
                isMove = true;  
            }
        }


        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1) * 3; 
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1) * 3;  
        }
    }


    private bool IsGrounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f);
        return hit.collider != null; 
    }

}

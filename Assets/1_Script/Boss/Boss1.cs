using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : Boss
{
    public Boss1Ui ui;
    private bool isAttack = true;
    public Animator animator;
    public GameObject skill1obj;

    public Transform center;
    public Transform[] attackTrans;

    public GameObject partic;
    public ParticleSystem particle;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private void Start()
    {
        bossName = "¸ô¶ó";
        maxHp = 100;

        damage = 50;
        animators = animator;

   
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(Attack1());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Attack2());
        }
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
                    }
                }

                break;
            }
        }
    }

    //public IEnumerator Attack3()
    //{

    //}
}

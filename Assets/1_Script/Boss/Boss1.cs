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
    }
    public IEnumerator Attack1()
    {
        StartCoroutine(ui.Attack1Ui());
        yield return StartCoroutine(ui.Attack1Ui());
        yield return new WaitForSeconds(0.4f);
        for(int i = 0; i < ui.attack1Image.Length; i++)
        {
            GameObject clone = Instantiate(skill1obj, ui.skilBool[i].position, Quaternion.identity);
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

    //public IEnumerator Attack2()
    //{

    //}
    //public IEnumerator Attack3()
    //{

    //}
}

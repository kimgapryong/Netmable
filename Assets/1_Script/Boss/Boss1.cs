using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Boss
{
    private bool isAttack = true;
    public Animator animator;
    public GameObject[] skill;
    private void Start()
    {
        bossName = "¸ô¶ó";
        maxHp = 100;

        damage = 50;
        animators = animator;
    }

   //public IEnumerator Attack1()
   // {
   //     if(isAttack)
   //     {
            
   //     }
   // }

   // public IEnumerator Attack2()
   // {

   // }
   // public IEnumerator Attack3()
   // {

   // }
}

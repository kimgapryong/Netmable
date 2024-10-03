using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1Ui : MonoBehaviour
{
    public GameObject attack1;
    public GameObject attack2;

    public Image[] attack1Image;
    public Image attack2Image;

    public Slider bossSlider;
    public Image bossImage;
    public Text bossName;
    public Text hpText;

    private Boss boss;

    public void GetBoss(Boss boss)
    {
        this.boss = boss;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    StartCoroutine(Attack1Ui());
        //}else if(Input.GetKeyDown(KeyCode.Alpha2)) { StartCoroutine(Attack2Ui()); }
        UpadateBossUi();
    }

    private void UpadateBossUi()
    {
        hpText.text = $"{boss.currentHp}/ {boss.maxHp}";
        bossSlider.value = (float)boss.currentHp / boss.maxHp;  
    }
    public IEnumerator Attack1Ui()
    {
        attack1.SetActive(true);
        for(int i = 0; i < attack1Image.Length; i++)
        {
            attack1Image[i].enabled = false;
        }
        for(int i =0; i < attack1Image.Length; i++)
        {
          
                attack1Image[i].enabled = true;
                yield return new WaitForSeconds(0.2f);
                attack1Image[i].enabled = false;
           
            yield return new WaitForSeconds(0.2f);
        }
        attack1.SetActive(false);
    }

    public IEnumerator Attack2Ui()
    {
        attack2.SetActive(true);
       
            attack2Image.enabled = true;
            yield return new WaitForSeconds(0.2f);
            attack2Image.enabled = false;
        
        attack2.SetActive(false);
    }
    
}

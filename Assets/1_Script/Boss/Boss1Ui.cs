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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Attack1Ui());
        }else if(Input.GetKeyDown(KeyCode.Alpha2)) { StartCoroutine(Attack2Ui()); }
    }
    public IEnumerator Attack1Ui()
    {
        attack1.SetActive(true);
        for(int i =0; i < attack1Image.Length; i++)
        {
            for (int j =0; j < 2; j++)
            {
                attack1Image[i].enabled = true;
                yield return new WaitForSeconds(0.2f);
                attack1Image[i].enabled = false;
            }
            yield return new WaitForSeconds(0.2f);
        }
        attack1.SetActive(false);
    }

    public IEnumerator Attack2Ui()
    {
        attack2.SetActive(true);
        for (int j = 0; j < 2; j++)
        {
            attack2Image.enabled = true;
            yield return new WaitForSeconds(0.2f);
            attack2Image.enabled = false;
        }
        attack2.SetActive(false);
    }
    
}

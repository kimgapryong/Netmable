using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public PlayerStatus status;

    public Text hp;
    public Text mp;

    public Slider hpSlider;
    public Slider mpSlider;
    public Slider exSlider;

    public Image[] no;
    public Image[] skil;

    public int nextSkil = 5;


    public void UpdatePlayerstatus()
    {
        hp.text = $"{status.currentHp}  / {status.maxHp}";
        mp.text = $"{status.currentMp}  / {status.maxMp}";

        
        if(status.currentLevel >= nextSkil)
        {
            for(int i = 0; i < skil.Length; i++)
            {
                no[i].enabled = false ;
                skil[i].enabled = true ;
                
            }
            nextSkil += nextSkil;
        }
    }
}

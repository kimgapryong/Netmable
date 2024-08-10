using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance { get; private set; }

    public PlayerStatus status;

    public Text hp;
    public Text mp;
    public Text ex;

    public Slider hpSlider;
    public Slider mpSlider;


    public Image[] no;
    public Image[] skil;


    public Button inventoryButton;
    public Button xButton;

    public GameObject inventory;

    public int nextSkil = 5;

    //∏ÛΩ∫≈Õ UI

    public Slider mSlider;
    public Image mImage;
    public Text mhpTxt;
    public Text mNameTxt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        UpdatePlayerstatus();
    }

    public IEnumerator UpdateMonsterUi(MonsterData monsterData, int maxHp, Func<int> getHealth)
    {
        mSlider.maxValue = maxHp;
        mNameTxt.text = monsterData.monsterName;

        while (true)
        {
            int currentHealth = getHealth();
            mSlider.value = currentHealth;
            mhpTxt.text = $"{currentHealth} / {maxHp}";
            yield return null;
        }
    }
    public void UpdatePlayerstatus()
    {
        hp.text = $"{status.currentHp}  / {status.maxHp}";
        mp.text = $"{status.currentMp}  / {status.maxMp}";
        ex.text = $"LEVEL: {status.currentLevel}";

        hpSlider.value = (float)status.currentHp / status.maxHp;
        mpSlider.value = (float)status.currentMp / status.maxMp;

        if (status.currentLevel >= nextSkil)
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

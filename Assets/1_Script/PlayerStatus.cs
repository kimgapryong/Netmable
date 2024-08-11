using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int damage;
    public float speed;
    public float jumpPower;

    public int currentHp, maxHp, currentMp, maxMp;

    public int currentLevel = 1, maxLevel = 25;

    public int currentEx;
    public int[] nextEx;

    private void Start()
    {
        nextEx = new int[maxLevel];
        nextEx[0] = 100;

        for(int i = 1; i < maxLevel; i++)
        {
            nextEx[i] = Mathf.RoundToInt(nextEx[i - 1] * 1.1f);
        }
    }

    public void AddLevel(int ex)
    {
        currentEx += ex;
        while (currentEx >= nextEx[currentLevel] && currentLevel < maxLevel)
        {
            LevelUp();
        }
        if (currentLevel >= maxLevel)
        {
            currentEx = nextEx[maxLevel];  // 최종 레벨에서 경험치가 누적되지 않도록 설정
        }
    }


    private void LevelUp()
    {
        currentLevel++;
        currentEx -= nextEx[currentLevel];

        maxHp += 100;
        currentHp = maxHp;

        maxMp += 20;
        currentMp = maxMp;
    }
}

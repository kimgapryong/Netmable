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

    public int currentLevel, maxLevel;

    public int currentEx;
    public int[] nextEx;

    private void Start()
    {
        nextEx = new int[maxLevel];
        nextEx[0] = 100;

        for(int i = 1; i <= maxLevel; i++)
        {
            nextEx[i] = Mathf.RoundToInt(nextEx[i - 1] * 1.1f);
        }
    }

    private void AddLevel(int ex)
    {
        currentEx += ex;
        if(currentEx >= nextEx[currentLevel] && currentLevel < maxLevel)
        {
            LevelUp();
        }
        if(currentLevel >= maxLevel)
        {
            currentEx = 0;
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

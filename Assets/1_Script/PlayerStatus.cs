using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public event Action OnLevelUp;
    public int nextSkil = 5;
    
    public int damage;
    public float speed;
    public float jumpPower;

    public int currentHp, maxHp, currentMp, maxMp;

    public int currentLevel = 0, maxLevel = 25;

    public int currentEx;
    public int[] nextEx;

    private void Start()
    {
        nextEx = new int[maxLevel];
        nextEx[0] = 100;

        for (int i = 1; i < maxLevel; i++)
        {
            nextEx[i] = Mathf.RoundToInt(nextEx[i - 1] * 1.1f);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddLevel(10);
        }
    }

    public void AddLevel(int ex)
    {
        currentEx += ex;

        while (currentLevel < maxLevel && currentEx >= nextEx[currentLevel])
        {
            LevelUp();
        }

        if (currentLevel >= maxLevel - 1)
        {
            currentEx = nextEx[maxLevel - 1];  // maxLevel - 1�� �����Ͽ� �ִ� �������� ����
            currentLevel = maxLevel - 1;  // ������ �ִ� ������ ����
        }
    }

    private void LevelUp()
    {
        if (currentLevel < maxLevel - 1) // maxLevel - 1�� �����Ͽ� �ִ� ���� �̻����� �������� �ʵ���
        {
            currentLevel++;
            currentEx -= nextEx[currentLevel - 1];

            OnLevelUp?.Invoke();

            maxHp += 30;
            currentHp = maxHp;

            maxMp += 20;
            currentMp = maxMp;

            damage += 5;
            speed += 0.5f;
        }
        else
        {
            currentLevel = maxLevel - 1;  // maxLevel - 1�� ����
            currentEx = nextEx[maxLevel - 1]; // ����ġ�� �ִ� �������� �� �̻� �������� �ʵ���
        }

    }


}

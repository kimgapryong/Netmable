using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private const string SAVECHAR = "#SAVE-VALUE#";
    string filepath = Application.dataPath + "/1_Script/Data/jsonData/savaFile.json";

    

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

        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            string[] jsonData = json.Split(new[] { SAVECHAR }, StringSplitOptions.None);
            // json�� ���� ������ ������
            damage = int.Parse(jsonData[0]);
            speed = float.Parse(jsonData[1]);
            jumpPower = float.Parse(jsonData[2]);
            maxHp = int.Parse(jsonData[3]);
            maxMp = int.Parse(jsonData[4]);
            currentLevel = int.Parse(jsonData[5]); 
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
    public bool ManaOk(int mana)
    {
        return currentMp - mana >= 0;
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

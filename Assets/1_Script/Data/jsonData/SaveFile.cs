using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    private const string SAVECHAR = "#SAVE-VALUE#";
    public PlayerStatus status;
    private void Start()
    {
        status = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            WriteFile();
        }
    }
    public void WriteFile()
    {
        Debug.Log("데이터 시작");
        SaveData saveData = new SaveData()
        {
            damage = status.damage,
            speed = status.speed,
            jumpPower = status.jumpPower,
            maxHp = status.maxHp,
            maxMp = status.maxMp,
            currentLevel = status.currentLevel
        };
  
        string[] contents = new string[]
        {
            ""+saveData.damage,
            "" + saveData.speed,
            "" + saveData.jumpPower,
            "" + saveData.maxHp,
            "" + saveData.maxMp,
            "" + saveData.currentLevel
        };
        string saveValue = string.Join(SAVECHAR, contents);
        Debug.Log(saveValue);
        File.WriteAllText(Application.dataPath + "/1_Script/Data/jsonData/savaFile.json", saveValue);
    }   
}
public class SaveData
{
    public int damage;
    public float speed;
    public float jumpPower;

    public int maxHp, maxMp;
    public int currentLevel;

}

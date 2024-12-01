using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ChooiseSceneData : MonoBehaviour
{   
    private string path = Application.dataPath + "/1_Script/Data/StageData/Stage.json";
    public GameObject[] stages;

  
    private void Start()
    {
        ReadFile();
    }
    //  처음 스테이지 버튼들 이미지 비활성화
    private void ResetStage()
    {
        for(int i = 0; i < stages.Length; i++)
        {
            stages[i].gameObject.SetActive(false);
        }
    }
    private void ReadFile()
    {
        ResetStage();
        if (File.Exists(path))
        {
            string stageData = File.ReadAllText(path);
            StageData stage = JsonUtility.FromJson<StageData>(stageData);

            for (int i = 0; i < stage.stageInt; i++)
            {
                stages[i].SetActive(true);
            }
        }
        
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("Stage1");
    }

}

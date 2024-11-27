
using System.IO;
using UnityEngine;

public class StageJsonManager : MonoBehaviour
{
    private const string STAGEVALUE = "#STAGE#";
    private string path = Application.dataPath + "/1_Script/Data/StageData/Stage.json";
    public static StageJsonManager Instance { get { return instance; } private set { } }
    private static StageJsonManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(instance);
        }
    }

    public void SaveStageData(int currentStage)
    {
        StageData stage = new StageData
        {
            stageInt = currentStage
        };
        string jsonData = JsonUtility.ToJson(stage);
        File.WriteAllText(path, jsonData);
    }
}

public class StageData
{
    public int stageInt = 0;
}

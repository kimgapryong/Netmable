
using UnityEngine;

public class StageJsonManager : MonoBehaviour
{
    private const string STAGEVALUE = "#STAGE#";
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

    public void SaveStageData()
    {

    }
}

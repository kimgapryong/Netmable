using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTuotorial : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Start-1");
    }
    public void CurrentGame()
    {
        SceneManager.LoadScene("ChooiseScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

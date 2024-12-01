using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExSaveBtnScript : MonoBehaviour
{
    public GameObject image;

    public void BtnBack()
    {
        if(image.activeSelf == true)
        {
            image.SetActive(false);
        }
        else
        {
            image.SetActive (true);
        }
    }

    public void ExGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public string[] speak;
    private Dialogue dialogue;
    public Transform[] trans;
    public GameObject[] Cha;

    
    private void Start()
    {
        dialogue = GameObject.Find("DialogueSystem").GetComponent<Dialogue>();
        dialogue.SetChat(speak, trans[0]);
    }

    private void Update()
    {
        SetChaTrans();
    }

    private void SetChaTrans()
    {
        for(int i = 0; i < Cha.Length; i++)
        {
            trans[i].transform.position = Cha[i].transform.position + new Vector3(0,1.5f,0);
        }
    }
}

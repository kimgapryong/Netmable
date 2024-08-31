using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private Queue<string> chat;

    private string currentString;
    private Transform obj;

    public GameObject chatObj;
    private TextMeshPro textMeshPro;

    public void SetChat(string[] lines, Transform trans)
    {
        chat = new Queue<string>();
        foreach (string line in lines)
        {
            chat.Enqueue(line);
        }
        obj = trans;
        StartCoroutine(chatSystem());
    }

    private IEnumerator chatSystem()
    {
        GameObject clone = Instantiate(chatObj, obj);
        textMeshPro = clone.GetComponentInChildren<TextMeshPro>();
        yield return null;
        while(chat.Count > 0)
        {
            currentString = chat.Dequeue();
            textMeshPro.text = currentString;

            yield return new WaitForSeconds(2);
        }
        Destroy(clone);
    }
    
}

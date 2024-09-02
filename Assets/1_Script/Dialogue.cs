using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private Queue<string> chat; // ��ȭ ť
    private string currentString;
    private Transform obj;

    public GameObject chatObj;
    public GameObject background;
    public TextMeshPro textMeshPro;

    private bool isChatting = false; // ��ȭ�� ���� ������ ����

    public void SetChat(string[] lines, Transform trans)
    {
        chat = new Queue<string>();
        chat.Clear();
        foreach (string line in lines)
        {
            chat.Enqueue(line);
        }
        obj = trans;
        StartCoroutine(chatSystem());
        
    }

    private IEnumerator chatSystem()
    {
        isChatting = true;
        GameObject clone = Instantiate(chatObj, obj);
        textMeshPro = clone.GetComponentInChildren<TextMeshPro>();
        background = textMeshPro.transform.Find("Background").gameObject;

        yield return null;

        while (chat.Count > 0)
        {
            currentString = chat.Dequeue();
            textMeshPro.text = currentString;
            float x = textMeshPro.preferredWidth;
          
            x = (x > 3) ? x + 0.7f : 3;
         
            background.transform.localScale = new Vector2(x, textMeshPro.preferredHeight + 0.5f);
            yield return new WaitForSeconds(2); // �� ��縦 2�� ���� ǥ��
        }

        Destroy(clone);
        isChatting = false; // ��ȭ�� �������� ǥ��
    }

    public bool IsChatFinished()
    {
        return !isChatting; // ��ȭ�� �������� Ȯ��
    }
}

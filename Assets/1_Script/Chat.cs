using System.Collections;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [SerializeField]
    public Chating[] speak; // ��ȭ �迭
    private Dialogue dialogue; // Dialogue �ý��� ����
    public Transform[] trans; // ĳ���� ��ġ �迭
    public GameObject[] Cha; // ĳ���� ������Ʈ �迭

    private int currentIndex = 0; // ���� ��ȭ �ε���

    private void Start()
    {
        dialogue = GameObject.Find("DialogueSystem").GetComponent<Dialogue>();
        StartCoroutine(Tutorial()); // Ʃ�丮�� ����
    }

    private void Update()
    {
        SetChaTrans();
    }

    private void SetChaTrans()
    {
        for (int i = 0; i < Cha.Length; i++)
        {
            trans[i].transform.position = Cha[i].transform.position + new Vector3(0, 1.5f, 0);
        }
    }

    private IEnumerator Tutorial()
    {
        while (currentIndex < speak.Length)
        {
            Chating currentChat = speak[currentIndex];
            dialogue.SetChat(currentChat.chat, trans[currentChat.index]);
            yield return new WaitUntil(() => dialogue.IsChatFinished()); // ��ȭ�� ���� ������ ���
            currentIndex++; // ���� ��ȭ�� �̵�
            yield return new WaitForSeconds(1); // ���� ��ȭ ���� �� ��� ��� (�ʿ��� ���)
        }
    }
}

[System.Serializable]
public struct Chating
{
    public int index;
    public string name;
    public string[] chat;

}

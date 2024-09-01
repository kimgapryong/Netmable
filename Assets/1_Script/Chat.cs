using System.Collections;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [SerializeField]
    public Chating[] speak; // 대화 배열
    private Dialogue dialogue; // Dialogue 시스템 참조
    public Transform[] trans; // 캐릭터 위치 배열
    public GameObject[] Cha; // 캐릭터 오브젝트 배열

    private int currentIndex = 0; // 현재 대화 인덱스

    private void Start()
    {
        dialogue = GameObject.Find("DialogueSystem").GetComponent<Dialogue>();
        StartCoroutine(Tutorial()); // 튜토리얼 시작
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
            yield return new WaitUntil(() => dialogue.IsChatFinished()); // 대화가 끝날 때까지 대기
            currentIndex++; // 다음 대화로 이동
            yield return new WaitForSeconds(1); // 다음 대화 시작 전 잠시 대기 (필요한 경우)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueChat;
    public Image characterIcon;
    public Image playerIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    private GameObject player;
    private Collider2D currentColl;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        player = GameObject.Find("Player");
        lines = new Queue<DialogueLine>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            DisplayNextDialogueLine();  
        }
    }
    public void StartDialogue(Dialogue dialogue, Collider2D collison)
    {
        currentColl = collison;
        isDialogueActive = true;
        player.SetActive(false);
        dialogueChat.SetActive(true);

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        // 대사가 남아있는지 먼저 체크
        if (lines.Count == 0)
        {
            EndDialogue();
           
            if(currentColl != null)
            {
                Destroy(currentColl.gameObject);
            }
            Debug.Log(lines.Count);
            return;
        }

        // 대사를 가져와서 출력
        DialogueLine currentLine = lines.Dequeue();
        characterName.text = currentLine.character.name;

        if (characterName.text == "주인공")
        {
            playerIcon.sprite = currentLine.character.icon;
            playerIcon.gameObject.SetActive(true);
            characterIcon.gameObject.SetActive(false);
        }
        else
        {
            characterIcon.sprite = currentLine.character.icon;
            characterIcon.gameObject.SetActive(true);
            playerIcon.gameObject.SetActive(false);
        }

        StopAllCoroutines();  // 기존에 실행 중인 코루틴 중단
        StartCoroutine(TypeSentence(currentLine));  // 새로운 대사 출력
    }


    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueChat.SetActive(false);
        player.SetActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private DialogueLine currentLine;


    public GameObject dialogueChat;
    public Image characterIcon;
    public Image playerIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    private GameObject player;
    private MovePlayer movePlayer;
    private PlayerAttack attackPlayer;
    private Collider2D currentColl;
    
    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    public GameObject moveBtn;
    public GameObject diffBtn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        player = GameObject.Find("Player");
        lines = new Queue<DialogueLine>();
        movePlayer = player.GetComponent<MovePlayer>();
        attackPlayer = player.GetComponent<PlayerAttack>();
        animator = player.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            DisplayNextDialogueLine();  
        }
        if (aginScrren && !currentLine.isEvent)
        {
            aginScrren = false;
            dialogueChat.SetActive(true);
            DisplayNextDialogueLine();
        }
    }
    public void StartDialogue(Dialogue dialogue, Collider2D collison)
    {
        movePlayer.horizontal = 0;
        currentColl = collison;
        isDialogueActive = true;
        //player.SetActive(false);
        movePlayer.enabled = false;
        attackPlayer.enabled = false;
        moveBtn.SetActive(false);
        diffBtn.SetActive(false);
        AnimatorControal(animator);
        animator.SetBool("isIdle", true);
        animator.SetBool("isGround", true);
        
        dialogueChat.SetActive(true);

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
            Debug.Log(dialogueLine);
        }
        currentLine = lines.Dequeue();
        chatSys(currentLine);
        //DisplayNextDialogueLine();
    }
    private void AnimatorControal(Animator anima)
    {
        for(int i = 0; i < anima.parameterCount; i++)
        {
            AnimatorControllerParameter param = anima.GetParameter(i);

            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
            
        }
    }

    bool canDialogue;
    bool typing = false;
    bool okEvent;
    bool aginScrren;
    public void DisplayNextDialogueLine()
    {
        //if (!canDialogue)
        //    return;

        // ��簡 �����ִ��� ���� üũ

        Debug.Log("start");
        Debug.Log(lines.Count);
        if (lines.Count == 0 )
        {
            EndDialogue();
           
            if(currentColl != null)
            {
                Destroy(currentColl.gameObject);
            }
            Debug.Log(lines.Count);
            return;
        }

        if (typing)
        {
            StopAllCoroutines();
            dialogueArea.text = currentLine.line;
            typing = false;


            if (currentLine != null)
            {
                if (currentLine.onEvent != null && currentLine.onEvent.GetPersistentEventCount() > 0)
                {
                    currentLine.onEvent?.Invoke(currentLine);
                    currentLine.isEvent = true;
                    aginScrren = true;
                    dialogueChat.SetActive(false);
                }

            }
        }
        else
        {

            // ��縦 �����ͼ� ���
            if (!currentLine.isEvent)
            {
                Debug.Log("��� ��� ����");
               
                // ��� ť���� ���� ��� ��������
                currentLine = lines.Dequeue();
               
                chatSys(currentLine);  // ��� ���
            }
        }

      
    }
    private void chatSys(DialogueLine dialogueLine)
    {
        characterName.text = dialogueLine.character.name;

        if (characterName.text == "���ΰ�")
        {
            playerIcon.sprite = dialogueLine.character.icon;
            playerIcon.gameObject.SetActive(true);
            characterIcon.gameObject.SetActive(false);
        }
        else
        {
            characterIcon.sprite = dialogueLine.character.icon;
            characterIcon.gameObject.SetActive(true);
            playerIcon.gameObject.SetActive(false);
        }
        

        StopAllCoroutines();  // ������ ���� ���� �ڷ�ƾ �ߴ�
        StartCoroutine(TypeSentence(dialogueLine));  // ���ο� ��� ���
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        typing = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typing = false;


        if (currentLine != null)
        {
            if (currentLine.onEvent != null && currentLine.onEvent.GetPersistentEventCount() > 0)
            {
                currentLine.onEvent?.Invoke(dialogueLine);
                currentLine.isEvent = true;
                aginScrren = true;
                dialogueChat.SetActive(false);
            }

        }

    }
 

    void EndDialogue()
    {
        //if (dialogueLine != null)
        //{
        //    dialogueLine.onEvent?.Invoke();
        //}
        
        isDialogueActive = false;
        dialogueChat.SetActive(false);
        movePlayer.enabled = true;
        attackPlayer.enabled = true;
        moveBtn.SetActive(true);
        diffBtn.SetActive(true);
        player.SetActive(true);
        Destroy(currentColl.gameObject);
    }
}
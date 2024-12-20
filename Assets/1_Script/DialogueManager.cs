using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using NUnit.Framework;

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

    public bool isChat;
    public CameraMove cam;
    private float camOrigin;

    public AudioClip chatClip;
    public List<GameObject> sourceList = new List<GameObject>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        player = GameObject.Find("Player");
        lines = new Queue<DialogueLine>();
        movePlayer = player.GetComponent<MovePlayer>();
        attackPlayer = player.GetComponent<PlayerAttack>();
        animator = player.GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraMove>();
        camOrigin = cam.smoothSpeed;

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
    public void StartDialogue(Dialogue dialogue, Collider2D collison = null)
    {

        if (collison != null)
        {
            currentColl = collison;
            collison.gameObject.SetActive(false);
        }
        for (int i = sourceList.Count - 1; i >= 0; i--)
        {
            if (sourceList[i] != null)
            {
                sourceList.RemoveAt(i);
            }
        }
        sourceList.Clear();
        SoundManager.Instance.canChat = true;
        isChat = true;
        cam.smoothSpeed = 1f;
        movePlayer.horizontal = 0;

        isDialogueActive = true;
        //player.SetActive(false);
        movePlayer.rigid.velocity = new Vector2(0, movePlayer.rigid.velocity.y); 
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
        if (currentLine.trans != null)
        {
            cam.player = currentLine.trans;
        }
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

        // 대사가 남아있는지 먼저 체크

      
        if (lines.Count == 0 )
        {
            SoundManager.Instance.canChat = false;
            EndDialogue();
           
            if(currentColl != null)
            {
                Destroy(currentColl.gameObject);
            }
            return;
        }

        if (typing)
        {
            StopAllCoroutines();
            sourceList.Clear();
            dialogueArea.text = currentLine.line;
            typing = false;


            if (currentLine != null)
            {
                if (currentLine.onEvent != null && currentLine.onEvent.GetPersistentEventCount() > 0)
                {
                    currentLine.isEvent = true;
                    currentLine.onEvent?.Invoke(currentLine);

                    aginScrren = true;
                    dialogueChat.SetActive(false);

                }

            }
        }
        else
        {

            // 대사를 가져와서 출력
            if (!currentLine.isEvent)
            {
                Debug.Log("대사 출력 시작");
               
                // 대사 큐에서 다음 대사 가져오기
                currentLine = lines.Dequeue();
                if(currentLine.trans != null)
                {
                    cam.player = currentLine.trans;
                }
                chatSys(currentLine);  // 대사 출력
            }
        }

      
    }
    private void chatSys(DialogueLine dialogueLine)
    {
        if(dialogueLine.character.name != null)
        {
            characterName.text = dialogueLine.character.name;
        }

        if (characterName.text == "주인공")
        {
            playerIcon.sprite = dialogueLine.character.icon;
            if(player != null)
            {
                cam.player = player.transform;
            }
            playerIcon.gameObject.SetActive(true);
            characterIcon.gameObject.SetActive(false);
        }
        else if(string.IsNullOrEmpty(characterName.text))
        {
            Debug.Log("1");
            characterIcon.gameObject.SetActive(false);
            playerIcon.gameObject.SetActive(false);
        }
        else if(characterName.text == "없음")
        {
            dialogueChat.SetActive(false);
        }
        else
        {
            Debug.Log("2");
            characterIcon.sprite = dialogueLine.character.icon;
            characterIcon.gameObject.SetActive(true);
            playerIcon.gameObject.SetActive(false);
        }
        

        StopAllCoroutines();  // 기존에 실행 중인 코루틴 중단
        StartCoroutine(TypeSentence(dialogueLine));  // 새로운 대사 출력
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        typing = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            SoundManager.Instance.ChatSound("Chat", chatClip);
            sourceList.Add(SoundManager.Instance.currentSFXSound);
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typing = false;


        if (currentLine != null)
        {
            if (currentLine.onEvent != null && currentLine.onEvent.GetPersistentEventCount() > 0)
            {
                currentLine.isEvent = true;
                currentLine.onEvent?.Invoke(dialogueLine);
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
        cam.smoothSpeed = camOrigin;
        isChat = false;
        if (player != null)
        {
            cam.player = player.transform;
        }
  
        if (currentColl != null)
        {
            Destroy(currentColl.gameObject);
        }
    }
}
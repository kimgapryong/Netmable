using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartImage : MonoBehaviour
{
    public Image image;
    public Image blackImage;
    public Dialogue dialogue;
    public Chat chat0;
    public DialogueManager manager;
    private Animator animator;
    private CameraMove cam;

    public AudioClip bgClip;
    public AudioClip fgClip;
    private void Start()
    {
        animator = image.GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraMove>();
        manager = DialogueManager.Instance;
        //blackImage.enabled = false;
        StartCoroutine(StartBalckScreen());
    }

    private IEnumerator WaitForAnimationCoroutine()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);

        while (animationState.normalizedTime < 1.0f)
        {
            animationState = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; // 매 프레임마다 대기
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("애니메이션이 끝났습니다.");
    }

    private IEnumerator CamShake(DialogueLine line)
    {
        Debug.Log("흔들림");
        StartCoroutine(cam.Shake(6f, 0.2f, 3f));
        yield return StartCoroutine(cam.Shake(1.2f, 0.2f, 3f));
        Debug.Log("안 흔들림");
        line.isEvent = false;
    }
    private IEnumerator StartBalckScreen()
    {
        yield return StartCoroutine(WaitForAnimationCoroutine());  
        image.gameObject.SetActive(false);
        blackImage.gameObject.SetActive(true);
        manager.StartDialogue(dialogue);
        //image.enabled = false ;
    }
    private IEnumerator DeleteBlackScreen(DialogueLine line)
    {
        yield return new WaitForSeconds(0.5f);
        blackImage.gameObject.SetActive(false);
        SoundManager.Instance.BGSound(bgClip);
        line.isEvent = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    public void StartShake(DialogueLine line)
    {
        SoundManager.Instance.SFXSound("fuking", fgClip);
        StartCoroutine(CamShake(line));
    }
    public void DestroyThis(DialogueLine line)
    {
        StartCoroutine(DeleteBlackScreen(line));
       
    }

    private void OnDestroy()
    {
        manager.StartDialogue(chat0.dialogue);
    }
}

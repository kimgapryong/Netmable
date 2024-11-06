using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartImage : MonoBehaviour
{
    public Image image;
    public Image blackImage;
    public Dialogue dialogue;
    public DialogueManager manager;
    private Animator animator;
    private CameraMove cam;
    private void Start()
    {
        animator = image.GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraMove>();
        blackImage.enabled = false;
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
        yield return new WaitForSeconds(1.5f);
        Debug.Log("애니메이션이 끝났습니다.");
    }

    private IEnumerator CamShake(DialogueLine line)
    {
        StartCoroutine(cam.Shake(1.2f, 0.2f, 0.7f));
        yield return StartCoroutine(cam.Shake(1.2f, 0.2f, 0.7f));
        line.isEvent = false;
    }
    private IEnumerator StartBalckScreen()
    {
        yield return StartCoroutine(WaitForAnimationCoroutine());  
        
        blackImage.enabled = true;
        manager.StartDialogue(dialogue);
        image.enabled = false ;
    }
    private IEnumerator DeleteBlackScreen(DialogueLine line)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(blackImage);
        line.isEvent = false;
    }
    public void StartShake(DialogueLine line)
    {
        StartCoroutine(CamShake(line));
    }
    public void DestroyThis(DialogueLine line)
    {
        StartCoroutine(DeleteBlackScreen(line));
       
    }
}

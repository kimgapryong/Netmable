using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartImage : MonoBehaviour
{
    public Image image;
    public Image blackImage;
    public Dialogue dialogue;
    public DialogueManager manager;
    private Animator animator;
    private DialogueLine cur;
    private void Start()
    {
        animator = image.GetComponent<Animator>();
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
    private IEnumerator StartBalckScreen()
    {
        yield return StartCoroutine(WaitForAnimationCoroutine());  
        
        blackImage.enabled = true;
        manager.StartDialogue(dialogue);
        image.enabled = false ;
    }

    public void DestroyThis(DialogueLine line)
    {
        cur = line;
        Destroy(blackImage);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        cur.isEvent = false;
    }
}

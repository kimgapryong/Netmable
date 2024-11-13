using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Chat : MonoBehaviour
{
    public Dialogue dia;
    public Animator animator;
    public AudioClip clip;
    private void Start()
    {
        DialogueManager.Instance.StartDialogue(dia);
        animator = GameObject.Find("Boss2").GetComponent<Animator>();
    }

    public void SupDone(DialogueLine line)
    {
        Camera.main.orthographicSize = 6f;
        SoundManager.Instance.BossSound("Dragon", clip);
        animator.SetTrigger("Dragon");
        StartCoroutine(GetAnimator(line));
    }
    private IEnumerator GetAnimator(DialogueLine line)
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);

        while (animationState.normalizedTime < 1.0f)
        {
            animationState = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; // 매 프레임마다 대기
        }
        yield return new WaitForSeconds(2f);
        line.isEvent = false;
        Destroy(gameObject);
    }
}

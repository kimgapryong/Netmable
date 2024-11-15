using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss2Chat : MonoBehaviour
{
    public Dialogue dia;
    public Animator animator;
    public AudioClip clip;
    public GameObject textMeshPro;
    private void Start()
    {
        DialogueManager.Instance.StartDialogue(dia);
        animator = GameObject.Find("Boss2").GetComponent<Animator>();
        UiManager.Instance.hpSlider.gameObject.SetActive(false);
        UiManager.Instance.mpSlider.gameObject.SetActive(false);
        UiManager.Instance.inventoryButton.gameObject.SetActive(false);
        UiManager.Instance.monsterUi.gameObject.SetActive(false);
        UiManager.Instance.playerName.gameObject.SetActive(false);
        UiManager.Instance.ex.gameObject.SetActive(false);
        GameObject.Find("Player").SetActive(false);
        Camera.main.transform.position = transform.position;

    }

    public void SupDone(DialogueLine line)
    {
        Camera.main.orthographicSize = 17f;
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
        Debug.Log("시작");
        StartCoroutine(StartStageScript.instance.FadeInEffect(UiManager.Instance.blackSrc));
        yield return StartCoroutine(StartStageScript.instance.FadeInEffect(UiManager.Instance.blackSrc));
        textMeshPro.SetActive(true);
        line.isEvent = false;
        Destroy(gameObject);
    }
}

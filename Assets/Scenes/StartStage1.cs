using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage1 : MonoBehaviour
{
    public AudioClip bgClip;
    public Dialogue dialogue;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        player.GetComponent<MovePlayer>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;
        UiManager.Instance.blackSrc.gameObject.SetActive(true);
        SoundManager.Instance.BGSound(bgClip);
        StartCoroutine(StartStageScript.instance.FadeInOutEffect(UiManager.Instance.blackSrc));
        StartCoroutine(startDia());
    }
    private IEnumerator startDia()
    {
        yield return StartCoroutine(StartStageScript.instance.FadeInOutEffect(UiManager.Instance.blackSrc));
        UiManager.Instance.blackSrc.gameObject.SetActive(false);
        if(dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    
}

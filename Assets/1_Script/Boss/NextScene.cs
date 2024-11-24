using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class NextScene : MonoBehaviour
{
    private PlayerStatus status;
    public Dialogue dia;
    public Image blackScreen;
    public Boss boss;
    public Boss1Ui ui;
    public Finding find;

    public StartStageScript stage;
    private GameObject player;
    private Vector2 bossOrigin;
    private Vector2 playerOrigin;
    private Vector2 originPos;
    private bool isDie = true;
    private void Start()
    {
        status = PlayerManager.Instance.playerStatus;
        stage = GameObject.Find("SceneFade").GetComponent<StartStageScript>();
        player = GameObject.Find("Player");
        blackScreen = UiManager.Instance.blackSrc;
        bossOrigin = boss.transform.localScale;
        playerOrigin = new Vector2(-9.5f, -1.6f);
        originPos = new Vector2(16f, 0.5f);
        PlayerManager.Instance.realDie = false;

    }
    private void Update()
    {
        if(status.currentHp <= 0 && isDie)
        {

            isDie = false;
            StartCoroutine(WaitStart());
            player.transform.position = playerOrigin;
            boss.GetComponent<Animator>().enabled = false;    
            boss.transform.localScale = bossOrigin;
            boss.transform.position = originPos;
           
        }
    }
    private IEnumerator WaitStart()
    {
        blackScreen.gameObject.SetActive(true);
        ui.gameObject.SetActive(false);
        find.isCanFind = false;
        find.enabled = false;
        boss.enabled = false;
        player.GetComponent<MovePlayer>().enabled = false;
        yield return new WaitForSeconds(1.8f);
        blackScreen.gameObject.SetActive(false);
        DialogueManager.Instance.StartDialogue(dia);
    }
    public void StartScreen(DialogueLine line)
    {

        StartCoroutine(stage.FadeInOutEffect(blackScreen, line));
    }
    public void NextPreparation(DialogueLine line)
    {
        status.currentHp = status.maxHp;
        line.isEvent = false;
        PlayerManager.Instance.realDie = true;
        SceneManager.LoadScene("Stage1");
    }
}

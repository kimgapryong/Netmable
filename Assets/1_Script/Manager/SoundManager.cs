using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sound;

    private bool isMoveing;
    public GameObject currentSoundObject;
    public GameObject currentSFXSound;
    private GameObject player;
    private MovePlayer movePlayer;
    public bool canChat = true;
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }
    private void Start()
    {
        player = GameObject.Find("Player");
        movePlayer = player.GetComponent<MovePlayer>();
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(movePlayer.isGround && !DialogueManager.Instance.isChat)
        {
            CURSound(isMoveing, movePlayer.isGround ? movePlayer.moveClip : null);
        }
        
    }
    public void BossSound(string soundName, AudioClip clip)
    {

        currentSFXSound = new GameObject(soundName + "Sound");
        AudioSource scorce = currentSFXSound.AddComponent<AudioSource>();
        scorce.clip = clip;
        scorce.volume = 0.4f;
        scorce.Play();

            Destroy(currentSFXSound, clip.length);



    }
    public void SFXSound(string soundName,AudioClip clip)
    {

        Destroy(currentSoundObject);

        if (currentSFXSound != null)
        {
            currentSFXSound.GetComponent<AudioSource>().volume = 0;
        }
        currentSFXSound = new GameObject(soundName + "Sound");
        AudioSource scorce = currentSFXSound.AddComponent<AudioSource>();
        scorce.clip = clip;
        scorce.volume = 0.4f;
        scorce.Play();
        if (currentSFXSound.name != "ChatSound")
        {
            Destroy(currentSFXSound, clip.length);
        }


    }
    public void ChatSound(string soundName, AudioClip clip)
    {

        if (canChat)
        {

            Destroy(currentSoundObject);

            if (currentSFXSound != null)
            {
                currentSFXSound.GetComponent<AudioSource>().volume = 0;
            }
            currentSFXSound = new GameObject(soundName + "Sound");
            AudioSource scorce = currentSFXSound.AddComponent<AudioSource>();
            scorce.clip = clip;
            scorce.volume = 0.4f;
            scorce.Play();
            if (currentSFXSound.name != "ChatSound")
            {
                Destroy(currentSFXSound, clip.length);
            }



        }
    }
    public void CURSound(bool isMove, AudioClip clip = null)
    {
        isMoveing = isMove;
        if (isMove  && movePlayer.isGround && clip != null)
        {
            if (currentSoundObject == null)  // 새로운 오디오 오브젝트가 없는 경우에만 생성
            {
                currentSoundObject = new GameObject("PlayerSound");
                AudioSource source = currentSoundObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = true;
                source.Play();
            }
        }
        // 움직임이 중지될 때
        else if (!isMove && currentSoundObject != null)
        {
            Destroy(currentSoundObject);  // 멤버 변수로 참조된 오디오 오브젝트를 삭제
            //currentSoundObject = null;    // 참조 해제
        }
    }
    public void BGSound(AudioClip clip)
    {
        sound.clip = clip;
        sound.loop = true;
        sound.volume = 0.1f;
        sound.Play();
    }
    public void StopBG(DialogueLine line)
    {
        sound.Stop();
        line.isEvent = false;
    }
    public void StartBG(DialogueLine line)
    {
        sound.Play();
        line.isEvent = false;
    }
}

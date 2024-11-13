using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartStageScript : MonoBehaviour
{
    public static StartStageScript instance{get; private set;}
    public Image image;
    public Image sImage1;
    public Image sImage2;
    public bool isStartStage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(sImage1 != null)
        {
            SetImageAlpha(sImage1, 0f);
        }
        if(sImage2 != null)
        {
            SetImageAlpha(sImage2, 0f);
        }
        if(isStartStage)
        {
            StartCoroutine(StartNext());
        }

    }

    public IEnumerator FadeInOutEffect(Image photo, DialogueLine line = null)
    {
        if (!photo.gameObject.activeSelf)
        {
            photo.gameObject.SetActive(true);
        }
        if(line != null)
        {
            SetImageAlpha(photo, 0f);
        }

        float duration = 2.0f; 
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration); 
            SetImageAlpha(photo, alpha);
            Debug.Log("1");
            yield return null;
        }


        yield return new WaitForSeconds(1.5f);

        if(line == null)
        {
            t = 0;
            while (t < duration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / duration); // alpha ���� 1���� 0���� ����
                SetImageAlpha(photo, alpha);
                Debug.Log("2");
                yield return null;
            }
        }
       
        if(line != null)
        {
            line.isEvent = false;
        }
        Debug.Log("�̹��� �����");
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color; 
    }

    private IEnumerator StartNext()
    {
        yield return StartCoroutine(WaitForAnimationCoroutine());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeInOutEffect(sImage1));
        yield return StartCoroutine(FadeInOutEffect(sImage1));
        StartCoroutine(FadeInOutEffect(sImage2));
        yield return StartCoroutine(FadeInOutEffect(sImage2));
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Start-1");
    }

    private IEnumerator WaitForAnimationCoroutine()
    {
        // �ִϸ��̼��� ���� ������ ���
        AnimatorStateInfo animationState = image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        while (animationState.normalizedTime < 1.0f)
        {
            animationState = image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            yield return null; // �� �����Ӹ��� ���
        }

        yield return new WaitForSeconds(1.5f);
        image.gameObject.SetActive(false);
        Debug.Log("�ִϸ��̼��� �������ϴ�.");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartStageScript : MonoBehaviour
{
    public Image image;
    public Image sImage1;
    public Image sImage2;

    private void Start()
    {
        sImage1.canvasRenderer.SetAlpha(0.0f); 
        sImage2.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(StartNext());
    }

    private IEnumerator FadeInOutEffect()
    {
        image.gameObject.SetActive(false);
        float duration = 1.0f; 
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration); 
            SetImageAlpha(sImage1, alpha); 
            SetImageAlpha(sImage2, alpha);
            yield return null;
        }


        yield return new WaitForSeconds(1.5f);

        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration); // alpha 값이 1에서 0으로 변함
            SetImageAlpha(sImage1, alpha);
            SetImageAlpha(sImage2, alpha);
            yield return null;
        }

        Debug.Log("이미지 사라짐");
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
        StartCoroutine(FadeInOutEffect());
    }

    private IEnumerator WaitForAnimationCoroutine()
    {
        // 애니메이션이 끝날 때까지 대기
        AnimatorStateInfo animationState = image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        while (animationState.normalizedTime < 1.0f)
        {
            animationState = image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            yield return null; // 매 프레임마다 대기
        }

        yield return new WaitForSeconds(1.5f);
        Debug.Log("애니메이션이 끝났습니다.");
    }
}

using UnityEngine;

public class Fader : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime = 1f;

    private Coroutine currentFadeCoroutine;



    public void FadeIn()
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(Fade(canvasGroup.alpha, 1f, fadeTime));
    }

    public void FadeOut()
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        currentFadeCoroutine = StartCoroutine(Fade(canvasGroup.alpha, 0f, fadeTime));
    }

    private System.Collections.IEnumerator Fade(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        currentFadeCoroutine = null;
    }
}

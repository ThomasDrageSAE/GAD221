using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    
    private bool isFading = false;

    public static event Action OnFadeInComplete;
    public static event Action OnFadeOutComplete;
    
    private void Awake()
    {
        EventSubscription();
        
        if (canvasGroup == null)
        {
            Debug.LogError("ScreenFade CanvasGroup has not been assigned.");

            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void OnDestroy()
    {
        EventUnsubscription();
    }

    private void EventSubscription()
    {
        DayManager.OnDayChanged += StartFadeIn;
        DayManager.OnDayEnded += StartFadeOut;
    }

    private void EventUnsubscription()
    {
        DayManager.OnDayChanged -= StartFadeIn;
        DayManager.OnDayEnded -= StartFadeOut;
    }

    public void StartFadeOut(int day)
    {
        StartFadeOut();
    }
    
    public void StartFadeOut()
    {
        if (isFading)
        {
            StopAllCoroutines();
        }
        
        isFading = true;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut");
        if (canvasGroup == null)
            yield break;

        canvasGroup.blocksRaycasts = false;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        OnFadeOutComplete?.Invoke();
        isFading = false;
    }

    public void StartFadeIn(int day)
    {
        if (isFading)
        {
            StopAllCoroutines();
        }
        
        isFading = true;
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
        if (canvasGroup == null)
            yield break;
        
        canvasGroup.blocksRaycasts = false;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        OnFadeInComplete?.Invoke();
        isFading = false;
    }
}
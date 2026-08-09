using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (canvasGroup == null)
        {
            Debug.LogError(
                "ScreenFade CanvasGroup has not been assigned.");

            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public IEnumerator FadeOut()
    {
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
    }

    public IEnumerator FadeIn()
    {
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
    }
}
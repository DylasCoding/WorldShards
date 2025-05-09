using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    private Image _sceneFadeImage;

    private void Awake()
    {
        _sceneFadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1f);
        Color endColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0f);
        yield return StartCoroutine(FadeCoroutine(startColor, endColor, duration));

        gameObject.SetActive(false);
    }

    public IEnumerator FadeOut(float duration)
    {
        gameObject.SetActive(true);
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0f);
        Color endColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1f);
        yield return StartCoroutine(FadeCoroutine(startColor, endColor, duration));

        gameObject.SetActive(true);
    }

    private IEnumerator FadeCoroutine(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;
        float elapsedPecentage = 0f;
        while (elapsedPecentage < 1)
        {

            elapsedPecentage = elapsedTime / duration;
            _sceneFadeImage.color = Color.Lerp(startColor, endColor, elapsedPecentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}

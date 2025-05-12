using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public float displayTime = .3f;
    public float fadeDuration = 0.2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void ShowNotification(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeNotification(message));
    }

    IEnumerator FadeNotification(string message)
    {
        notificationText.text = message;

        // Fade In
        yield return StartCoroutine(FadeCanvasGroup(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(displayTime);

        // Fade Out
        yield return StartCoroutine(FadeCanvasGroup(1f, 0f, fadeDuration));
    }

    IEnumerator FadeCanvasGroup(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpDamage : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private TextMeshProUGUI damageText;

    void Awake()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Color startColor = damageText.color;
        while (elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lifeTime;

            // Move up
            transform.position = startPosition + new Vector3(0, moveSpeed * Time.deltaTime, 0);

            // Fade out
            damageText.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0, t * fadeSpeed));

            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}

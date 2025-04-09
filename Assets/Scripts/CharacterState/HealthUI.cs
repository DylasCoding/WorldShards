using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI; // Add this namespace

public class HealthUI : MonoBehaviour
{
    private Image barImage;
    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();

        barImage.fillAmount = 1f;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        //reset before update
        barImage.fillAmount = 0f;
        float healthPercentage = CalculateFillAmount(currentHealth, maxHealth);
        barImage.fillAmount = healthPercentage;
    }

    private float CalculateFillAmount(int currentHealth, int maxHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;
        //round to #.##f
        fillAmount = Mathf.Round(fillAmount * 100f) / 100f;
        return fillAmount;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image fillImage;

    private int maxHealth;

    public void SetMaxHealth(int max)
    {
        maxHealth = max;
        SetHealth(max);
    }

    public void SetHealth(int currentHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public Image boss;
    public float bossRestante = 100f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bossRestante <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CuraRecebida(20);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DanoRecebido(20);
        }
    }
    public void DanoRecebido(float dano)
    {
        bossRestante -= dano;
        boss.fillAmount = bossRestante / 100f;
    }
    public void CuraRecebida(float cura)
    {
        bossRestante += cura;
        bossRestante = Mathf.Clamp(bossRestante, 0, 100);

        boss.fillAmount = bossRestante / 100f;
    }
}

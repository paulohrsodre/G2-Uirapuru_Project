using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image vida;
    public float vidaRestante = 100f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vidaRestante <= 0)
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
        vidaRestante -= dano;
        vida.fillAmount = vidaRestante / 100f;
    }
    public void CuraRecebida(float cura)
    {
        vidaRestante += cura;
        vidaRestante = Mathf.Clamp(vidaRestante, 0, 100);

        vida.fillAmount = vidaRestante / 100f;
    }

}

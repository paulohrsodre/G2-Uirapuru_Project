using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public static ItemCollector Instance;

    public int totalItemsCollect;
    private int currentCollected;

    public Text collectedText;
    public GameObject bossObject;

    [Header("Sounds Settings")]
    public AudioSource audioBoss;
    public AudioSource ambientMusic;
    public AudioClip newAmbientMusic;

    private bool hasTriggerEvent = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        bossObject.SetActive(false);
    }

    public void Collect()
    {
        currentCollected++;
        UpdateUI();

        if(currentCollected >= totalItemsCollect && !hasTriggerEvent)
        {
            hasTriggerEvent = true;

            if(audioBoss != null)
            {
                audioBoss.Play();
            }

            bossObject.SetActive(true);

            if(ambientMusic != null && newAmbientMusic != null)
            {
                ambientMusic.Stop();
                ambientMusic.clip = newAmbientMusic;
                ambientMusic.Play();
            }
        }
    }

    private void UpdateUI()
    {
        collectedText.text = $"Cestos: {currentCollected}/{totalItemsCollect}";
    }
}

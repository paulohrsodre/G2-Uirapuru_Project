using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public int itemID;
    public AudioSource itemAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemCollector.Instance.Collect();
            itemAudio.Play();
            Destroy(gameObject);
        }
    }
}

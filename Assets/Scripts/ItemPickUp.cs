using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal
}

public class ItemPickUp : MonoBehaviour
{
    public ItemType itemType;

    [Header("Heal Settings")]
    public int healAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if(player != null)
            {
                switch (itemType)
                {
                    case ItemType.Heal:
                        player.Heal(healAmount);
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : MonoBehaviour
{
    public int damage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if(player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}

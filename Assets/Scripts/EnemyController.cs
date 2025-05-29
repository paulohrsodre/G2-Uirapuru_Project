using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speedEnemy;
    public int maxHealth;
    public int damageInPlayer;

    [Header("Drop Settings")]
    [Range(0, 100)]
    public float dropChance;
    public GameObject projectilePickupPrefab;

    private int currentHealth;
    private bool isChasing = false;
    private Vector2 originalPosition;
    private Transform player;

    private Rigidbody2D rig;

    private Vector3 spawnPosition => transform.position;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalPosition = transform.position;
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && isChasing)
        {
            ChasePlayer();
        }
        else
        {
            ReturnToPosition();
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rig.MovePosition((Vector2)transform.position + direction * speedEnemy * Time.deltaTime);
    }

    private void ReturnToPosition()
    {
        Vector2 direction = (originalPosition - (Vector2)transform.position);

        if (direction.magnitude > 0.01f)
        {
            rig.MovePosition((Vector2)transform.position + direction.normalized * speedEnemy * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        rig.velocity = Vector2.zero;

        DropPickUp();
        Destroy(gameObject, 2f);
    }

    private void DropPickUp()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= dropChance && projectilePickupPrefab != null)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * 0.5f;
            Vector3 dropPosition = transform.position + new Vector3(offset.x, offset.y, 0f);

            Instantiate(projectilePickupPrefab, dropPosition, projectilePickupPrefab.transform.rotation);

            Debug.Log("Drop offset: " + offset);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(damageInPlayer);
            }
        }
    }
}

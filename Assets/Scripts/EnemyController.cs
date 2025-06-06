using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speedEnemy;
    public int maxHealth;
    public int damageInPlayer;
    public Image healthBar;

    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        [Range(0f, 100f)] public float dropChance;
    }

    [Header("Drop Settings")]
    public List<DropItem> dropItems = new List<DropItem>();

    private int currentHealth;
    private bool isChasing = false;
    private bool isDead = false;
    private Vector2 originalPosition;
    private Transform player;

    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private Vector3 spawnPosition => transform.position;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalPosition = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        Vector2 moveDir = Vector2.zero;

        if (player != null && isChasing)
        {
            moveDir = ChasePlayer();
        }
        else
        {
            moveDir = ReturnToPosition();
        }

        bool isWalking = moveDir.magnitude > 0.1f;

        anim.SetBool("walk", isWalking);

        if(isWalking)
        {
            anim.SetFloat("axisX", moveDir.x);
            anim.SetFloat("axisY", moveDir.y);
        }
    }

    private Vector2 ChasePlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rig.MovePosition((Vector2)transform.position + direction * speedEnemy * Time.deltaTime);
        return direction;
    }

    private Vector2 ReturnToPosition()
    {
        Vector2 direction = (originalPosition - (Vector2)transform.position);

        if (direction.magnitude > 0.01f)
        {
            Vector2 dirNorm = direction.normalized;
            rig.MovePosition((Vector2)transform.position + dirNorm * speedEnemy * Time.deltaTime);
            return dirNorm;
        }

        return Vector2.zero;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        UpdateHealthBar();

        StartCoroutine(RedFlash());

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        rig.velocity = Vector2.zero;

        anim.SetTrigger("death");

        DropPickUp();
        Destroy(gameObject, 2f);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float normalizedHealth = (float)currentHealth / maxHealth;
            healthBar.fillAmount = normalizedHealth;
        }
    }

    private void DropPickUp()
    {
        foreach (DropItem drop in dropItems)
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= drop.dropChance && drop.itemPrefab != null)
            {
                Vector2 offset = Random.insideUnitCircle.normalized * 0.5f;
                Vector3 dropPosition = transform.position + new Vector3(offset.x, offset.y, 0f);

                Instantiate(drop.itemPrefab, dropPosition, drop.itemPrefab.transform.rotation);
                Debug.Log($"Dropped {drop.itemPrefab.name} at offset {offset}, roll: {roll}, chance: {drop.dropChance}");
            }
        }
    }

    IEnumerator RedFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
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
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 dir = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;

            anim.SetFloat("axisX", dir.x);
            anim.SetFloat("axisY", dir.y);
            anim.SetTrigger("attack");

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(damageInPlayer);
            }
        }
    }
}

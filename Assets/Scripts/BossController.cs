using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float speedBoss;
    public int maxHealth;
    public int damageInPlayer;
    public Image healthBar;

    [Header("Drop Settings")]
    [Range(0, 100)]
    public float dropChance;
    public GameObject projectilePickupPrefab;

    private int currentHealth;
    private bool isDead = false;
    private Transform player;

    private Rigidbody2D rig;
    private Animator anim;

    [Header("Others")]
    [SerializeField] private GameObject dialogueTriggerPrefab;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private PlayerController playerController;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (isDead || player == null)
            return;

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rig.MovePosition((Vector2)transform.position + direction * speedBoss * Time.deltaTime);

        anim.SetBool("walk", direction.magnitude > 0.1f);

        if (direction != Vector2.zero)
        {
            anim.SetFloat("axisX", direction.x);
            anim.SetFloat("axisY", direction.y);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

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

        GameObject trigger = Instantiate(dialogueTriggerPrefab, transform.position, Quaternion.identity);
        FinalDialogTirgger triggerScript = trigger.GetComponent<FinalDialogTirgger>();
        triggerScript.dialogManager = dialogueManager;
        triggerScript.player = playerController;
    }

    private void DropPickUp()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= dropChance && projectilePickupPrefab != null)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * 0.5f;
            Vector3 dropPosition = transform.position + new Vector3(offset.x, offset.y, 0f);

            Instantiate(projectilePickupPrefab, dropPosition, projectilePickupPrefab.transform.rotation);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float normalizedHealth = (float)currentHealth / maxHealth;
            healthBar.fillAmount = normalizedHealth;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

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

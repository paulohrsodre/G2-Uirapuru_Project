using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float damageFlashDuration;

    [Header("Projectile Settings")]
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public int maxProjectile;
    private int currentProjectile;

    [Header("Melee Attack Settings")]
    public Transform attackPoint;
    public float attackRanger;
    public float attackOffset;
    public LayerMask enemyLayer;

    [Header("Health Settings")]
    public int maxHealth;
    private int currentHealth;

    [Header("UI Settings")]
    public Text projectileText;
    public Image healthFull;
    public Image healthBack;
    public GameObject deathPanel;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioSource meleeClip;
    public AudioSource shootClip;

    private Rigidbody2D rig;
    private Animator anim;
    private Vector2 lastDirection = Vector2.down;
    private SpriteRenderer spriteRenderer;
    private bool isWalk = false;
    private bool isShooting = false;
    private bool isDead = false;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentProjectile = maxProjectile;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthBar();

        if(deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!canMove)
        {
            rig.velocity = Vector2.zero;
            return;
        }

        Walk();
        Attack();
        Shooting();

        projectileText.text = currentProjectile.ToString();
    }

    public void Walk()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y);

        rig.velocity = moveDir * speed;

        isWalk = moveDir != Vector2.zero;

        if (isWalk)
        {
            lastDirection = moveDir.normalized;

            Vector2Int dir = new Vector2Int(Mathf.RoundToInt(lastDirection.x), Mathf.RoundToInt(lastDirection.y));

            attackPoint.localPosition = new Vector3(dir.x, dir.y, 0) * attackOffset;
        }

        anim.SetFloat("axisX", lastDirection.x);
        anim.SetFloat("axisY", lastDirection.y);
        anim.SetBool("walk", isWalk);

        if (isWalk)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if(audioSource.isPlaying && audioSource.clip == walkClip)
            {
                audioSource.Stop();
            }
        }
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("attack");
            meleeClip.Play();
        }
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire2") && !isShooting)
        {
            isShooting = true;
            anim.SetTrigger("shoot");
            shootClip.Play();
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRanger, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(1);
                continue;
            }

            BossController boss = enemy.GetComponent<BossController>();
            
            if(boss != null)
            {
                boss.TakeDamage(1);
                continue;
            }
        }
    }

    public void ProjectileFire()
    {
        Debug.Log("Disparou flecha");

        if (currentProjectile <= 0)
        {
            return;
        }

        if (lastDirection == Vector2.zero)
        {
            lastDirection = Vector2.down;
        }

        currentProjectile--;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        projectile.GetComponent<Rigidbody2D>().velocity = lastDirection * projectileSpeed;

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public void EndShoot()
    {
        isShooting = false;
    }

    public void AddProjectile(int amount)
    {
        currentProjectile = Mathf.Clamp(currentProjectile + amount, 0, maxProjectile);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        StartCoroutine(RedFlash());

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        rig.isKinematic = true;
        anim.SetTrigger("death");

        StartCoroutine(ShowDeathPanelWithDelay(1.5f));
    }

    private void UpdateHealthBar()
    {
        float normalizedHealth = (float)currentHealth / maxHealth;
        healthFull.fillAmount = normalizedHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthBar();
    }

    IEnumerator ShowDeathPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;

        if(deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }

    IEnumerator RedFlash()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(damageFlashDuration);

        spriteRenderer.color = Color.white;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRanger);
    }
}

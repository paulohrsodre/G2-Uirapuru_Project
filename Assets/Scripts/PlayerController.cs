using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;

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

    private Rigidbody2D rig;
    private Animator anim;
    private Vector2 lastDirection = Vector2.down;
    private bool isWalk = false;
    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentProjectile = maxProjectile;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Attack();
        Shooting();

        //projectileText.text = "Disparos: " + currentProjectile;
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
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("attack");
        }
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire2") && !isShooting)
        {
            isShooting = true;
            anim.SetTrigger("shoot");
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
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
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
        SceneManager.LoadScene(0);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierScript : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float moveSpeed = 2f;
    public int minDamage = 5;
    public int maxDamage = 10;
    public float attackCooldown = 1.5f;
    public int maxHealth = 20;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Collider2D soldierCollider;
    private Animator animator;  // Reference to the Animator component
    private int currentHealth;
    private GameObject targetEnemy;
    private Vector2 spawnPoint;
    private bool canAttack = true;
    private bool isDead = false;

    public event System.Action OnSoldierDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        animator = GetComponent<Animator>(); // Get the Animator component
        soldierCollider = GetComponent<Collider2D>();
    }

    public void SetSpawnPoint(Vector2 position)
    {
        spawnPoint = position;
    }

    private void Update()
    {
        if (isDead) return;

        if (targetEnemy == null || Vector2.Distance(transform.position, targetEnemy.transform.position) > detectionRange)
        {
            FindTarget();
        }

        if (targetEnemy != null)
        {
            MoveTowardsEnemy();

            if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRange && canAttack)
            {
                StartCoroutine(AttackEnemy());
            }
        }
        else
        {
            ReturnToSpawnPoint();
        }

        // Update animation state
        UpdateAnimationState();
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        float closestDistance = detectionRange;
        targetEnemy = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemies"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = hit.gameObject;
                }
            }
        }
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator AttackEnemy()
    {
        canAttack = false;
        animator.SetTrigger("Attack");  // Trigger the Attack animation

        if (targetEnemy != null)
        {
            int damage = Random.Range(minDamage, maxDamage);
            var enemyCtrl = targetEnemy.GetComponent<Enemy_Ctrl>();
            if (enemyCtrl != null)
            {
                enemyCtrl.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        animator.ResetTrigger("Attack");
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);  // Set the IsDead parameter in Animator
        soldierCollider.enabled = false;
        if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject);
        }

        OnSoldierDeath?.Invoke();
        Destroy(gameObject);
    }

    private void ReturnToSpawnPoint()
    {
        if (Vector2.Distance(transform.position, spawnPoint) > 0.1f)
        {
            Vector2 direction = (spawnPoint - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, spawnPoint, moveSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimationState()
    {
        if (isDead)
        {
            animator.SetBool("IsDead", true);
        }
        else if (targetEnemy != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
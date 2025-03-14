using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float arrowspeed = 5f;
    [SerializeField] private int arrowdmg = 10;

    public float lifeTime;
    private Transform target;

    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;

        Destroy(gameObject, lifeTime);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * arrowspeed;
    }
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    Enemy_Ctrl enemy = other.gameObject.GetComponent<Enemy_Ctrl>();
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(dmg);
    //    }

    //    //other.gameObject.GetComponent<Enemy_Ctrl>().TakeDamage(arrowdmg);

    //    //Destroy(gameObject);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            Enemy_Ctrl enemy = collision.gameObject.GetComponent<Enemy_Ctrl>();
            if (enemy != null)
            {
                enemy.TakeDamage(arrowdmg);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}

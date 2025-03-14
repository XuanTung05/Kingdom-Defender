using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class archertower: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator animator;



    [Header("Attribute")]
    [SerializeField] private float TargetingRange = 5f;
    [SerializeField] private float rotationspeed = 5f;
    [SerializeField] private float bps = 1f;

    private Transform Target;
    private float timeUntilFire;

    private void Update()
    {
        if (Target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardTarget();

        if (!CheckTargetIsInRange())
        {
            Target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("shooting");
        GameObject arrowObj = Instantiate(arrowPrefab, firingPoint.position, Quaternion.identity);
        arrow arrowScript = arrowObj.GetComponent<arrow>();
        arrowScript.SetTarget(Target);
        
    }


    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, TargetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            Target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(Target.position, transform.position) <= TargetingRange;
    }

    private void RotateTowardTarget()
    {
        float angle = Mathf.Atan2(Target.position.y - transform.position.y, Target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }



    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, TargetingRange);

    }
}

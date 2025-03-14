using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Arrow fly trajectory.
/// </summary>
public class dan : MonoBehaviour
{
   
    [HideInInspector] public int damage = 20;
   
    public float lifeTime = 3f;
    // Starting speed
    public float speed = 3f;
   
    public float speedUpOverTime = 0.5f;
   
    public float hitDistance = 0.2f;
  
    public float ballisticOffset = 0.5f;
    
    public bool freezeRotation = false;
   
   
   
    private Vector2 originPoint;
   
    public Transform target;
   
    private Vector2 aimPoint;
    
    private Vector2 myVirtualPosition;
  
    private Vector2 myPreviousPosition;
    
    private float counter;

   
    
    

    

    public void Fire(Transform target)
    {
      
        
        
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = target.position;
    
       Destroy(gameObject, lifeTime);
    }

    
    void FixedUpdate()
    {
     
        counter += Time.fixedDeltaTime;
        // Add acceleration
        speed += Time.fixedDeltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = target.position;
        }
        // Calculate distance from firepoint to aim
        Vector2 originDistance = aimPoint - originPoint;
        // Calculate remaining distance
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        // Move towards aim
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        // Add ballistic offset to trajectory
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        // Rotate bullet towards trajectory
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;

        if (distanceToAim.magnitude <= hitDistance)
        {
            HitTarget();
            Destroy(gameObject);
        }
     }

    private void HitTarget()
    {
        // Kiểm tra va chạm và gây sát thương cho mục tiêu
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, hitDistance);
        if (hitCollider != null)
        {
            // Chỉ xử lý sát thương cho các đối tượng có component Enemy_Ctrl
            Enemy_Ctrl enemy = hitCollider.GetComponent<Enemy_Ctrl>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }



    private void LookAtDirection2D(Vector2 direction)
    {
        if (!freezeRotation)//freezeRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

  
    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            // Calculate sinus offset
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            // Add offset to trajectory
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            
            return myVirtualPosition;
        }
    }
}

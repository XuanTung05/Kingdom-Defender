using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public int health;
    public float speed;
    public GameObject vfx;
    private void Update()
    {
        transform.Translate (Vector2.right * speed* Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bl") 
        {
            if (health > 0) 
            {
                health--;
            }

            if (health <= 0) 
            {
                Destroy(gameObject);
                return;
            }

            theodoichuot.Instances.tongtien += 100;
            GameObject vfxhit = Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(vfxhit, 0.5f);
           
        }
    }
}

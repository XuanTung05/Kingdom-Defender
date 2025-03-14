using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawtower : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject tower;
    
    void Start()
    {
        StartCoroutine(delaysp());
    }

    // Update is called once per frame
    IEnumerator delaysp() 
    {
        yield return new WaitForSeconds(2);
        Instantiate(tower, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

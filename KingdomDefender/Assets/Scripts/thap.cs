using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class thap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bluletpre;
    [SerializeField] private Transform posspawn;
    public Transform target;
    [SerializeField] private float distances;
    private float timedelayfire;

    public int lv;

    Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    private void Update()
    {
        find();
        if (target == null)
            return;
        if (timedelayfire <= 0f)
        {

            anim.Play("shoot");
            GameObject bl = Instantiate(bluletpre, posspawn.transform.position, Quaternion.identity);
            bl.GetComponent<dan>().Fire(target);
            timedelayfire = 3f;
        }
        else {
            timedelayfire -= Time.deltaTime;
        }
    }



    void find()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemies");

        foreach (GameObject currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        if (closestEnemy != null)
        {

            if (Vector3.Distance(transform.position, closestEnemy.transform.position) < distances)
            {

                target = closestEnemy.transform;
            }
        }

        if (target != null) 
        {
            if (transform.position.x > target.transform.position.x)
            {
                transform.GetChild(lv).GetChild(0).GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
            }
            else {
                transform.GetChild(lv).GetChild(0).GetChild(0).transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    public void uplv()
    {
        if (theodoichuot.Instances.tongtien < 100)
            return;
        //check du tien up lv ko


        if (lv >=2)
            return;
            lv++;
        
        for (int i = 0; i < transform.childCount; i++)
        { 
            transform.GetChild(i).gameObject.SetActive(false);
        }
        theodoichuot.Instances.tongtien -= 100;
        transform.GetChild(lv).gameObject.SetActive(true);
        anim = GetComponentInChildren<Animator>();
    }
}

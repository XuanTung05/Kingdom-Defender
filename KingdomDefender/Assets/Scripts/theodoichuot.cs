using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class theodoichuot : MonoBehaviour, IPointerDownHandler
{
    public static theodoichuot Instances { get; private set; }
    // Start is called before the first frame update
    [SerializeField] private GameObject anhthapdcchon;

   public int tongtien;

    [SerializeField] private Button btchonmuathap;
    private bool dangchonthap;
    [SerializeField] private GameObject thapprefabs;

    public Text textmoney;
    [SerializeField]
    private GameObject enemy;
    void Awake() {
        if (Instances == null) 
        {
            Instances = this;
        }

        InvokeRepeating("spawnenemy", 1, 3);
    }

    void spawnenemy() 
    { 
        Vector2 pos = Vector2.zero;
        pos.x = -10;
        pos.y += Random.Range(-2f, 2f);
        GameObject enmy = Instantiate(enemy, pos,Quaternion.identity);
        Destroy(enmy,20);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.name == "bt")
        {

            if (tongtien < 200)
                return;
            
            if (dangchonthap == true)
                return;

            dangchonthap = true;
            anhthapdcchon.SetActive(true);
        }
    }

   

    private void Update()
    {
        textmoney.text = "" + tongtien;
        if (dangchonthap == true)
        {
            anhthapdcchon.transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }

       
        if (Input.GetMouseButtonUp(0))
        {
            if (dangchonthap == true)
            {
               dangchonthap = false;
                anhthapdcchon.gameObject.SetActive(false);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    Debug.Log("Target Position: " + hit.point);
                    if (hit.collider.gameObject.tag != "Player")
                    {
                        tongtien -= 200;
                        Instantiate(thapprefabs, hit.point, Quaternion.identity);
                        return;
                    }
                    else {
                        Debug.Log("Target Position: co thap o day");
                    }
                }
                return;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (dangchonthap == false)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    Debug.Log("Target Position: " + hit.point);
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if (hit.collider.gameObject.GetComponent<thap>() != null)
                        {
                            hit.collider.gameObject.GetComponent<thap>().uplv();
                        }
                        return;
                    }
                }
            }
            return;

        }
    }
}

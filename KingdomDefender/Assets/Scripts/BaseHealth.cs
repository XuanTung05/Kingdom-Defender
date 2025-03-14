using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;
    public GameObject panel, text, button;

    public Text healthText;

    public int currentHeart => currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            TakeDamage(1);
            //Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(int dmg)
    {
        if(currentHealth > 0)
        {
            currentHealth -= dmg;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            UpdateHealthUI();
        }
    }
    private void Die()
    {
        panel.SetActive(true);
        button.SetActive(true);
        text.SetActive(true);
        Time.timeScale = 0;
    }
    private void UpdateHealthUI()
    {
        healthText.text = " " + currentHealth.ToString();
    }
}

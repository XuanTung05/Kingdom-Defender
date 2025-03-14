using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySetting : MonoBehaviour
{
    [Header("Money Setting")]
    [SerializeField] private int startingMoney = 100;
    private int currentMoney;

    [Header("UI Elements")]
    [SerializeField] Text moneyText;
    [SerializeField] Color insufficientFundsColor = Color.red;
    [SerializeField] float flashDuration = 0.5f;

    private Color originColors;
    [SerializeField] private int[] towerCost;

    private void Awake()
    {
        originColors = moneyText.color;
    }
    private void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyDisplay();
    }
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyDisplay();
        originColors = moneyText.color;
    }
    public bool TrySpendMoney(int amount)
    {
        if(currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyDisplay();
            return true;
        }
        else
        {
            StartCoroutine(FlashInsufficientFunds());
            return false;
        }
    }
    public int GetTowerCost(int index)
    {
        if (index >= 0 && index < towerCost.Length)
            return towerCost[index];
        else
            return 0;
    }
    private void UpdateMoneyDisplay()
    {
        moneyText.text = "Coin: " + currentMoney.ToString();
    }
    private IEnumerator FlashInsufficientFunds()
    {
        Color initialColor = moneyText.color;

        moneyText.color = insufficientFundsColor;
        yield return new WaitForSeconds(flashDuration);

        moneyText.color = initialColor;
    }
    public void ResetMoneyTextColor()
    {
        moneyText.color = originColors;
    }
    public void SetMoneyTextColor(Color color)
    {
        moneyText.color = color;
    }
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBuildTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private TowerSelectionUI towerSelectionUI;
    [SerializeField] private MoneySetting moneySetting;
    [SerializeField] private GameObject upgradeSellPanel;

    private bool isOccupied = false;
    private GameObject tower;
    private Color startColor;
    private int currentIndexUpgrade = -1;

    private void Start()
    {
        startColor = sr.color;
        upgradeSellPanel.SetActive(false);
        towerSelectionUI.OnTowerSelected -= BuildTower;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (isOccupied)
        {
            ShowUpgradeSellPanel();
        }
        else
        {
            towerSelectionUI.ShowTowerSelection();
            towerSelectionUI.OnTowerSelected += BuildTower;
        }
    }
    private void BuildTower(int towerIndex)
    {
        if (!isOccupied)
        {
            if (BuildManager.Main.TrySpendMoney(moneySetting.GetTowerCost(towerIndex)))
            {
                GameObject towerToBuild = BuildManager.Main.GetSelectedTower(towerIndex);
                tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
                towerSelectionUI.HideTowerSelection();
                isOccupied = true;
                moneySetting.ResetMoneyTextColor();
                currentIndexUpgrade = towerIndex;
            }
            else
            {
                StartCoroutine(FlashInsufficientFunds());
            }
        }
    }
    private void UpgradeTower(int upgradeIndex)
    {
        if (isOccupied && tower != null)
        {
            int upgradeCost = BuildManager.Main.GetUpgradeCost(upgradeIndex);

            if (BuildManager.Main.TrySpendMoney(upgradeCost))
            {
                Destroy(tower);
                GameObject upgradeTower = BuildManager.Main.GetUpgradeTower(upgradeIndex);
                tower = Instantiate(upgradeTower, transform.position, Quaternion.identity);
                moneySetting.ResetMoneyTextColor();

                HideUpgradeSellPanel();
            }
            else
            {
                StartCoroutine(FlashInsufficientFunds());
            }
        }
    }
    private void ShowUpgradeSellPanel()
    {
        Debug.Log("ShowUpgradeSellPanel called");

        upgradeSellPanel.SetActive(true);

        if (upgradeSellPanel != null)
        {
            Button sellButton = upgradeSellPanel.transform.Find("SellButton")?.GetComponent<Button>();
            Button upgradeButton = upgradeSellPanel.transform.Find("UpgradeButton")?.GetComponent<Button>();

            if (sellButton != null)
            {
                sellButton.onClick.RemoveAllListeners();
                sellButton.onClick.AddListener(SellTower);
            }
            else
            {
                Debug.LogError("SellButton không được tìm thấy trong upgradeSellPanel.");
            }
            if(upgradeButton != null)
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(() =>
                {
                    int upgradeIndex = currentIndexUpgrade;
                    UpgradeTower(upgradeIndex);
                });
            }
        }
        else
        {
            Debug.LogError("upgradeSellPanel không được gán hoặc không tồn tại.");
        }
    }

    private void HideUpgradeSellPanel()
    {
        upgradeSellPanel.SetActive(false);
    }
    private void SellTower()
    {
        if(tower != null)
        {
            int sellAmount = Mathf.RoundToInt(moneySetting.GetTowerCost(0) * 0.25f);
            moneySetting.AddMoney(sellAmount);
            Destroy(tower);
            isOccupied = false;
            HideUpgradeSellPanel();
            towerSelectionUI.OnTowerSelected -= BuildTower;
        }
    }
    private IEnumerator FlashInsufficientFunds()
    {
        moneySetting.SetMoneyTextColor(Color.red);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = startColor;

        moneySetting.ResetMoneyTextColor();
    }
}

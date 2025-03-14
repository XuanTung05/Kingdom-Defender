using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Main;
    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private GameObject[] upgradeTowerPrefabs;
    [SerializeField] private MoneySetting moneySetting;
    [SerializeField] private int[] upgradeCost;

    private int selectedTower = 0;

    private void Awake()
    {
        Main = this;
    }
    public GameObject GetSelectedTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Length)
            return towerPrefabs[index];
        else
            return null;
    }
    public GameObject GetUpgradeTower(int index)
    {
        if(index >= 0 && index <= upgradeTowerPrefabs.Length)
           return upgradeTowerPrefabs[index];
        else
            return null;
        
    }
    public bool TrySpendMoney(int amount)
    {
        return moneySetting.TrySpendMoney(amount);
    }
    public int GetUpgradeCost(int index)
    {
        if (index >= 0 && index < upgradeCost.Length)
            return upgradeCost[index];
        else
            return 0;
    }
}

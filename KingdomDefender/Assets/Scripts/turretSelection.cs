using UnityEngine;

public class turretSelection : MonoBehaviour
{
    public static turretSelection instance;

    private SlotBuildTower currentBuildSpot;
    public GameObject[] turretPrefabs;
    public int[] turretCosts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBuildSpot(SlotBuildTower buildSpot)
    {
        currentBuildSpot = buildSpot;
    }

    public void SelectTurret(int turretIndex)
    {
        if (currentBuildSpot == null || turretIndex >= turretPrefabs.Length)
            return;

        
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        currentBuildSpot = null;
    }
}
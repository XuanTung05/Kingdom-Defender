using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject towerSelectionPanel; 
    [SerializeField] private Button[] towerButtons; 

    public delegate void TowerSelected(int towerIndex);
    public event TowerSelected OnTowerSelected; 

    private void Start()
    {
        foreach (Button button in towerButtons)
        {
            button.onClick.AddListener(() =>
            {
                int towerIndex = button.transform.GetSiblingIndex();
                SelectTower(towerIndex);
            });
        }
    }

    private void SelectTower(int towerIndex)
    {
        OnTowerSelected?.Invoke(towerIndex); 
    }

    public void ShowTowerSelection()
    {
        towerSelectionPanel.SetActive(true);
    }

    public void HideTowerSelection()
    {
        towerSelectionPanel.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class BuildTowerButton : MonoBehaviour
{
    [Header("References")]
    public GameObject towerPrefab;
    public Transform buildPosition;

    private Button button;
    private GameObject builtTower;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnBuildTower);
        UpdateButtonState();
    }

    private void OnBuildTower()
    {
        // Check if there's already a tower built
        if (builtTower == null)
        {
            // Instantiate a new tower and store its reference
            builtTower = Instantiate(towerPrefab, buildPosition.position, Quaternion.identity);
            UpdateButtonState();  // Update the button state after building the tower
        }
        else
        {
            Debug.LogWarning("A tower is already built at this position.");
        }
    }

    private void UpdateButtonState()
    {
        // Disable the button if a tower is already built, enable it otherwise
        button.interactable = builtTower == null;
    }
}
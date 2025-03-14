using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button levelSceneButton;

    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        levelSceneButton.onClick.AddListener(LevelSceneButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void LevelSceneButton()
    {
        SceneManager.LoadScene("Level Menu");
    }
    public void VictoryPanel()
    {
        victoryPanel.SetActive(true);
    }
}

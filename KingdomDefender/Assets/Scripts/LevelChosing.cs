using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChosing : MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelsButton;
    public GameObject loadingPanel;
    public Slider progressBar;

    public Image[] starImages;
    public Sprite grayStar;
    public Sprite goldStar;
    public Text starCountText;
    private void Awake()
    {
        ButtonToArray();
        int unlockLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockLevel; i++)
        {
            buttons[i].interactable = true;
        }

        UpdateStarDisplay();
    }
    private void UpdateStarDisplay()
    {
        int currentStars = PlayerPrefs.GetInt("CurrentStars", 0);

        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < currentStars)
            {
                starImages[i].sprite = goldStar;
                starImages[i].enabled = true;
            }
            else
            {
                starImages[i].sprite = grayStar;
                starImages[i].enabled = true;
            }
        }

        starCountText.text = $"Stars: {currentStars}";
    }
    public void OpenLevel(int lvlId)
    {
        string levelName = "Level " + lvlId;
        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {

        loadingPanel.SetActive(true);


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);


        progressBar.value = 0;
        progressBar.maxValue = 1;


        while (!asyncLoad.isDone)
        {

            progressBar.value = asyncLoad.progress;


            yield return null;
        }


        loadingPanel.SetActive(false);
    }

    void ButtonToArray()
    {
        int count = levelsButton.transform.childCount;
        buttons = new Button[count];
        for (int i = 0; i < count; i++)
        {
            buttons[i] = levelsButton.transform.GetChild(i).GetComponent<Button>();
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameConfirm : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text welcomeText;
    public TMP_Text notificationText;
    public GameObject nameInputPanel;
    public GameObject welcomePanel;
    public Button[] otherButtons;
    public Button submitNameButton;
    public Button logoutButton;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", string.Empty);

        if (string.IsNullOrEmpty(playerName))
        {
            nameInputPanel.SetActive(true);
            welcomePanel.SetActive(false);
            SetButtonsInteractable(false);
        }
        else
        {
            welcomeText.text = $"Welcome, {playerName}!";
            nameInputPanel.SetActive(false);
            welcomePanel.SetActive(true);
            SetButtonsInteractable(true);
        }

        submitNameButton.onClick.AddListener(OnSubmitName);
    }

    public void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            notificationText.text = "Please enter a name.";
            return;
        }

        if (IsNameTaken(playerName))
        {
            notificationText.text = "This name is already taken.";
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", playerName);

            welcomeText.text = $"Welcome, {playerName}!";
            nameInputPanel.SetActive(false);
            welcomePanel.SetActive(true);

            SetButtonsInteractable(true);
            notificationText.text = "Name successfully set!";
        }
    }

    private bool IsNameTaken(string name)
    { 
        return name == "Player1";
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach (Button button in otherButtons)
        {
            button.interactable = interactable;
        }
    }
    public void OnLogout()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("token");

        SceneManager.LoadScene("LogIn_SignIn");
    }
}

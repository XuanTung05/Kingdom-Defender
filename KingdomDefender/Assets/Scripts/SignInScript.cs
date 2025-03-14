using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SignInScript : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField passwd;
    public TextMeshProUGUI notification;

    public void SigninButton()
    {
        StartCoroutine(Signin());
    }
    private IEnumerator Signin()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user.text);
        form.AddField("passwd", passwd.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangnhap.php", form);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            notification.text = "Failed to connect";
        }
        else
        {
            string get = www.downloadHandler.text;

            if(get == "empty")
            {
                notification.text = "Need to write down all the empty";
            }
            else if (get == "" || get == null)
            {
                notification.text = "User name or Password incorrect";
            }
            else if (get.Contains("Lỗi"))
            {
                notification.text = "Failed to connect the server";
            }
            else
            {
                notification.text = "Sign in successfully!";
                PlayerPrefs.SetString("token", get);

                PlayerPrefs.DeleteKey("PlayerName");

                StartCoroutine(NextScene());
            }
        }
    }
    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TitleScene");
    }
}

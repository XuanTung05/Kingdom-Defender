using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LogInScript : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField passwd;
    public TextMeshProUGUI notification;

    public void LoginButton()
    {
        StartCoroutine(Login());
    }
    private IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user.text);
        form.AddField ("passwd", passwd.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", form);
        yield return www.SendWebRequest();

        if(!www.isDone)
        {
            notification.text = "Log in not successful";
        }
        else if(www.isDone)
        {
            string get = www.downloadHandler.text;

            switch(get)
            {
                case "exist": notification.text = "Account already exist"; break;
                case "OK": notification.text = "Log in successful. Please return and Sign in"; break;
                case "ERROR": notification.text = "Log in failed"; break;
                default: notification.text = "Failed to connect server"; break;
            }
        }
    }
}

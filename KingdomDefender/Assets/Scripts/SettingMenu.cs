using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] GameObject settingMenu;

    public void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void ToggleMusic()
    {
        AudioManage.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManage.Instance.ToggleSFX();
    }

    public void MusicVolumn()
    {
        AudioManage.Instance.MusicVolumn(_musicSlider.value);
    }

    public void SFXVolumn()
    {
        AudioManage.Instance.SFXVolumn(_sfxSlider.value);
    }
}

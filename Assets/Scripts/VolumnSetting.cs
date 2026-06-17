using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public Slider sfxSlider;
   

    private void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.value = AudioManager.instance.GetSFXVolume();
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

       
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.instance.SetSFXVolume(value);
    }

    public void SetBGMVolume(float value)
    {
        AudioManager.instance.SetBGMVolume(value);
    }
}
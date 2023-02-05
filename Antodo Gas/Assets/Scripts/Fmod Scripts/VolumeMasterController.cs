using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeMasterController : MonoBehaviour
{

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;
    [SerializeField] Slider globalSlider;

    public void updateMusicVolume()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicMasterVolume", musicSlider.value);
    }

    public void updateFXVolume()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FXMasterVolume", fxSlider.value);
    }

    public void updateGlobalVolume()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GlobalMasterVolume", globalSlider.value);
    }
}

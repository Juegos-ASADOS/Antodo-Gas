using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Fmod_Engine : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference EnginePath;

    [SerializeField] public string MotorIntensity;                        


    FMOD.Studio.EventInstance engineEvent;


    void Start()
    {
        engineEvent = FMODUnity.RuntimeManager.CreateInstance(EnginePath);
    }

    public void playEngine()
    {
        engineEvent.start();
        engineEvent.release();
    }

    public void updateBoostMusic(float motorIntensityValue)
    {
        engineEvent.setParameterByName(MotorIntensity, motorIntensityValue);
    }
}

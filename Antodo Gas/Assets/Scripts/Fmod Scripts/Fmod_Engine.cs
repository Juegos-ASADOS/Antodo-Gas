using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_Engine : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference EnginePath;
    [SerializeField] public EventReference Engine1Path;

    [SerializeField] public string MotorIntensity;                        


    FMOD.Studio.EventInstance engineEvent;
    FMOD.Studio.EventInstance engineEvent1;


    void Start()
    {
        engineEvent = FMODUnity.RuntimeManager.CreateInstance(EnginePath);
        engineEvent1 = FMODUnity.RuntimeManager.CreateInstance(Engine1Path);
    }

    public void playEngine()
    {
        engineEvent.start();
        engineEvent1.start();
        engineEvent.release();
        engineEvent1.release();
    }

    public void updateBoostMusic(int motorIntensityValue)
    {
        engineEvent.setParameterByName(MotorIntensity, motorIntensityValue);
        engineEvent1.setParameterByName(MotorIntensity, motorIntensityValue);
    }
}

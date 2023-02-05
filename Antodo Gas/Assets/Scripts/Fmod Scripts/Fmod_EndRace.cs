using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_EndRace : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference metaSoundPath;
    [SerializeField] public EventReference endRacePath;


    public void playmetaSound()
    {
        FMOD.Studio.EventInstance changeRootEvent = FMODUnity.RuntimeManager.CreateInstance(metaSoundPath);
        changeRootEvent.start();
        changeRootEvent.release();
    } 
    
    public void endRaceSound()
    {
        FMOD.Studio.EventInstance changeRootEvent = FMODUnity.RuntimeManager.CreateInstance(endRacePath);
        changeRootEvent.start();
        changeRootEvent.release();
    }
}

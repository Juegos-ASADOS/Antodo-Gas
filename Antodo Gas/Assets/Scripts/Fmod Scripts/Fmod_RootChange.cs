using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_RootChange : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference ChangeRootPath;


    public void playChangeRoot()
    {
        FMOD.Studio.EventInstance changeRootEvent = FMODUnity.RuntimeManager.CreateInstance(ChangeRootPath);
        changeRootEvent.start();
        changeRootEvent.release();
    }
}

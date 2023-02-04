using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_UI_Button : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] public EventReference UIEventPath;

    [SerializeField] public string ButtonEventLength;                         // Saves the name of parameter length [Short, Mid, Long]
    [SerializeField] public string ButtonEventType;                           // Saves the name of type of button [Correct, Wrong]
    [SerializeField] public string ButtonEventInteractions;                   // Saves the name of type of button [Correct, Wrong]


    [SerializeField] private int buttonLengthInt;
    [SerializeField] private int buttonTypeInt;

    public void playButton()
    {
        FMOD.Studio.EventInstance UIEvent = FMODUnity.RuntimeManager.CreateInstance(UIEventPath);
        UIEvent.setParameterByName(ButtonEventLength, buttonLengthInt);
        UIEvent.setParameterByName(ButtonEventType, buttonTypeInt);
        UIEvent.setParameterByName(ButtonEventInteractions, 0);
        UIEvent.start();
        UIEvent.release();
    }
    public void HoveredButton()
    {
        FMOD.Studio.EventInstance UIEvent = FMODUnity.RuntimeManager.CreateInstance(UIEventPath);
        UIEvent.setParameterByName(ButtonEventLength, buttonLengthInt);
        UIEvent.setParameterByName(ButtonEventType, buttonTypeInt);
        UIEvent.setParameterByName(ButtonEventInteractions, 1);
        UIEvent.start();
        UIEvent.release();
    }
}

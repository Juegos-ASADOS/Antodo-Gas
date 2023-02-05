using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Fmod_Music : MonoBehaviour
{

    [Header("FMOD Settings")]
    [SerializeField] public EventReference MusicEventPath;
    [SerializeField] public EventReference MusicEffectsEventPath;

    [SerializeField] public string RaceStarted;
    [SerializeField] public string BoostConcatenation;

    FMOD.Studio.EventInstance MusicEvent;
    FMOD.Studio.EventInstance MusicEvent1;

    // Start is called before the first frame update
    void Start()
    {
        MusicEvent = FMODUnity.RuntimeManager.CreateInstance(MusicEventPath);
        MusicEvent1 = FMODUnity.RuntimeManager.CreateInstance(MusicEffectsEventPath);
    }

    public void playMusic() {
        MusicEvent.start();
        MusicEvent1.start();
        MusicEvent.release();
        MusicEvent1.release();
    }
    public void stopMusic() {
        MusicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        MusicEvent1.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void updateStartedMusic(bool started)
    {
        MusicEvent.setParameterByName(RaceStarted, started ? 1 : 0);
        MusicEvent1.setParameterByName(RaceStarted, started ? 1 : 0);
    }

    public void updateBoostMusic(int numBoost)
    {
        float aux;
        MusicEvent.getParameterByName(BoostConcatenation,out aux);
        int aux2 = (int)aux + numBoost;
        MusicEvent.setParameterByName(BoostConcatenation, aux2);
        Debug.Log(aux2);
    }

    public void resetBoostMusic()
    {
        MusicEvent.setParameterByName(BoostConcatenation, 0);

    }
}

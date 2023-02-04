using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodMusic : MonoBehaviour
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

    public void updateStartedMusic(bool started)
    {
        MusicEvent.setParameterByName(RaceStarted, started ? 0 : 1);
        MusicEvent1.setParameterByName(RaceStarted, started ? 0 : 1);
    }

    public void updateBoostMusic(int numBoost)
    {
        MusicEvent.setParameterByName(BoostConcatenation, numBoost);
    }
}

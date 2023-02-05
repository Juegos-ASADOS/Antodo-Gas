using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicaapelo : MonoBehaviour
{
    [SerializeField] Fmod_Music musica;
    // Start is called before the first frame update
    void Start()
    {
        musica.playMusic();
    }

    private void OnDestroy()
    {
        musica.stopMusic();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Fmod_Music musica;
    private void Start()
    {
        musica.playMusic();
    }
    //Exit the game
    public void Exit()
    {
        Application.Quit();
    }
}

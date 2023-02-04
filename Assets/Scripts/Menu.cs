using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //Exit the game
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exiting app\n");
    }
}

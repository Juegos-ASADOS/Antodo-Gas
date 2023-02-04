using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartGame : MonoBehaviour
{
    public void startGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}

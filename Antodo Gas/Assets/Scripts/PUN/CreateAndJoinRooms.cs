using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text textBox;

    public void CreateRoom()
    {
        string gmText = GameManager.instance.createLobby(createInput.text);

        textBox.text = gmText;
        if (gmText == "Creating Lobby...")
            PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        string gmText = GameManager.instance.enterLobby(joinInput.text);

        textBox.text = gmText;
        if(gmText == "Connecting to lobby...")
            PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingToPlay");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingPlayerNum : MonoBehaviour
{
    private TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void Update()
    {
       text.text = GameManager.instance.getNumPlayersInCurrentLobby().ToString();
    }
}

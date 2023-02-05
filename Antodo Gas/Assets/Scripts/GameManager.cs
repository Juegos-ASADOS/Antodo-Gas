using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Numerics;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const int maxPlayersInLobby = 4;

    Dictionary<string, int> playersInLobby = new Dictionary<string, int>();
    List<GameObject> playerObjects = new List<GameObject>(maxPlayersInLobby);
    int playerIndex;
    bool host = false;
    bool raceStarted = false;

    string currentLobby;

    public bool isMultiplayer { get; set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void setHost(bool t)
    {
        host = t;
    }
    public bool getHost()
    {
        return host; 
    }
    public void setRaceStarted(bool t)
    {
        raceStarted = t;
    }
    public bool getRaceStarted()
    {
        return raceStarted;
    }
    public static void changeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void setMultiplayer(bool state)
    {
        isMultiplayer = state;
    }

    public string enterLobby(string key)
    {
        int numPlayers;
        if (playersInLobby.TryGetValue(key, out numPlayers))
        {
            //SI no esta llena
            if (numPlayers < 4)
            {
                playersInLobby.Remove(key);
                playersInLobby.Add(key, numPlayers+1);
                currentLobby = key;
                return "Connecting to lobby...";
            }
            //Si esta llena
            return "Lobby Full!";
        }
        else
        {
            //Si no existe
            return "Lobby not found";
        }
    }
    public string createLobby(string key)
    {
        int numPlayers;
        if (playersInLobby.TryGetValue(key, out numPlayers))
        {
            //SI ya existe
            return "Lobby already exists!";
        }
        else
        {
            //Si no existe
            playersInLobby.Add(key, 1);
            currentLobby = key;
            return "Creating Lobby...";
        }
    }
    public int getNumPlayersInCurrentLobby()
    {
        int num;
        playersInLobby.TryGetValue(currentLobby, out num);
        return num;
    }
    public void addPlayer(GameObject cont)
    {
            Debug.Log("add");
        if (cont.GetComponent<PhotonView>().IsMine)
        {
            playerIndex = playerObjects.Count;
        }
        playerObjects.Add(cont);
    }
    public int getPosition()
    {
        int pos = 0;
        float myDistance = playerObjects[playerIndex].GetComponent<DemoCameraBezier>().getDistance();

        for(int i = 0; i < playerObjects.Count; ++i)
        {
            if (i == playerIndex)
                continue;

            if (myDistance < playerObjects[i].GetComponent<DemoCameraBezier>().getDistance())
                pos++;
        }
        return pos;
    }

    public List<GameObject> getPlayerObjects()
    {
        return playerObjects;
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const int maxPlayersInLobby = 4;

    Dictionary<string, int> playersInLobby = new Dictionary<string, int>();

    string currentLobby;
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
    public void changeScene(string name)
    {
        SceneManager.LoadScene(name);
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
}
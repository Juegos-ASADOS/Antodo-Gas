using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spawnPoint;
    [SerializeField]
    float x, y, z;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.addPlayer(PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(x, y, z), Quaternion.identity));
    }

}

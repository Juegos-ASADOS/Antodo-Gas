using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckStart : MonoBehaviour
{

    PhotonView view;
    DemoCameraBezier demoBez;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.getHost())
            this.enabled = false;

        view = GetComponent<PhotonView>();

        demoBez = GetComponent<DemoCameraBezier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine && demoBez.enabled)
        {
            GameManager.instance.setRaceStarted(true);
        }
        else if (view.IsMine && GameManager.instance.getRaceStarted())
        {
            demoBez.enabled = true;
            this.enabled = false;
        }
    }
}

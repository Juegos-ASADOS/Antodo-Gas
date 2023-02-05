using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckStart : MonoBehaviour
{

    PhotonView view;
    DemoCameraBezier demoBez;
    Vector3 oriTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.setRaceStarted(false);
        view = GetComponent<PhotonView>();

        demoBez = GetComponent<DemoCameraBezier>();
        oriTransform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(view.IsMine + " " + GameManager.instance.getRaceStarted());
        if (view.IsMine && GameManager.instance.getRaceStarted())
        {
            Debug.Log("aaaaaaaaaafdjatdfj");
            demoBez.startButton();
            this.enabled = false;
            return;
        }

        if (!view.IsMine && oriTransform != transform.position)
        {
            Debug.Log("soy el host");
            GameManager.instance.setRaceStarted(true);
        }
       
    }
}

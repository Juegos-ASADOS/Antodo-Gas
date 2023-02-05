using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public DemoCameraBezier demoBez;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.getHost())
            gameObject.SetActive(false);
    }
    public void StartRace()
    {
        demoBez.enabled = true;
        Destroy(this.gameObject);
    }
}

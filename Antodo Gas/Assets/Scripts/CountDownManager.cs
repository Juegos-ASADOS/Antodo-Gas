using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{

    public float TIMETOBEAT = 60.0f;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject textWin, textFalse;
    [SerializeField]
    GameObject button;

    [SerializeField]
    GameObject meta;

    bool win = false;

    // Update is called once per frame
    void Update()
    {
        if (TIMETOBEAT > 0.0f && !win)
        {
            TIMETOBEAT -= Time.deltaTime;
            if (TIMETOBEAT < 0.0f)
            {
                TIMETOBEAT = 0.0f;
                win = false;
                button.SetActive(true);
                textFalse.SetActive(true);
            }
        }
        else
        {
            if (player.GetComponent<DemoCameraBezier>().enabled)
                player.GetComponent<DemoCameraBezier>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Meta>() != null)
        {
            win = true;
            button.SetActive(true);
            textWin.SetActive(true);
        }
    }
}

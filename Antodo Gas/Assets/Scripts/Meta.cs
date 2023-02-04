using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Meta : MonoBehaviour
{
    int numPlayersFinished;
    GameManager gm;
    [SerializeField]
    GameObject finalText;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        numPlayersFinished++;
        if (other.transform.GetChild(0).gameObject.activeSelf){
            string text = "Winner";
            if (numPlayersFinished != 1)
                text = numPlayersFinished.ToString() + "º posición";
            finalText.GetComponent<TextMeshProUGUI>().SetText(text);
            finalText.SetActive(true);
        }
        if (numPlayersFinished >= gm.getNumPlayersInCurrentLobby())
            GameManager.changeScene("EndGame");
    }
}

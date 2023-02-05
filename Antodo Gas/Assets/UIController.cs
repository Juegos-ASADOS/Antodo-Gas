using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    string[] posTexts = { "1st", "2nd", "3rd", "4th" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int pos = GameManager.instance.getPosition();
        text.text = posTexts[pos];
    }
}

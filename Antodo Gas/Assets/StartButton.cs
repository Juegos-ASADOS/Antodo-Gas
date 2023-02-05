using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.getHost())
            gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColorPicker : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public Material[] materials;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_BaseColor", fcp.color);
        }
    }
}

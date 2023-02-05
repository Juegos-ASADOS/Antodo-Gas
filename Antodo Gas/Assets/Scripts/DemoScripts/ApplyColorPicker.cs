using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColorPicker : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public Material material;
    public Renderer[] gameObjects;

    private Material materialCopy;

    private void Start()
    {
        materialCopy = new Material(material);
    }

    // Update is called once per frame
    void Update()
    {
        materialCopy.SetColor("_BaseColor", fcp.color);
        for(int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].material = materialCopy;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
public class PinchoTool : MonoBehaviour
{
    public PathCreator culva;
    public float angle = 0; //eje pos el otro el z
    public float offSet = 4.0f;
    private float CoordPrev = 0;

    // Start is called before the first frame update
    void Start()
    {
        culva = this.transform.GetComponentInParent<PathCreator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //angle += (transform.localPosition.y - CoordPrev) * 10;
        //CoordPrev = transform.localPosition.y;
    }
}

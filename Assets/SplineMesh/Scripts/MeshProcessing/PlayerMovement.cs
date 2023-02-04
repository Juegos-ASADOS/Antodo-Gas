using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Spline spline;
    private float rate = 0;
    public float DurationInSecond;

    // Start is called before the first frame update
    void Start()
    {
        rate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rate);
        rate += Time.deltaTime / DurationInSecond;
        if (rate > spline.nodes.Count - 1)
        {
            rate -= spline.nodes.Count - 1;
        }
        Place();
    }


    private void Place()
    {
            CurveSample sample = spline.GetSample(rate);
            this.transform.localPosition = sample.location + (sample.up * (sample.scale.x / 3.0f));
            this.transform.localRotation = sample.Rotation;
    }
}

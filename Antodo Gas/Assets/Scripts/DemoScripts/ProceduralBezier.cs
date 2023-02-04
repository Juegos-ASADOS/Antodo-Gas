using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreationEditor;

public class ProceduralBezier : MonoBehaviour
{
    PathCreator pathCreator;
    Vector3 [] pathPoints;

    public float min = 10.0f, max = 30.0f;

    public float timer = 10.0f, accTime = 0.0f;


    private void Start()
    {
        pathPoints = new Vector3[] { new Vector3(2,0,0), new Vector3(15, 10, 0) , new Vector3(30, 0, 0), new Vector3(40, 20, 4), new Vector3(60, 4, -5)};
        pathCreator = GetComponent<PathCreator>();
        BezierPath path = new BezierPath(pathPoints, false, PathSpace.xyz);
        path.FlipNormals = true;
        pathCreator.bezierPath = path;
    }

    private void Update()
    {
        accTime += Time.deltaTime;
        if(accTime >= timer)
        {
            accTime = 0;
            CreateNewPath();
        }
    }

    private void CreateNewPath()
    {
        for(int i = 0; i < pathPoints.Length - 1; i++){
            pathPoints[i] = pathPoints[i + 1];
        }
        Vector3 last = pathPoints[pathPoints.Length - 1];
        pathPoints[pathPoints.Length - 1] = new Vector3(last.x + Random.Range(min, max), 
                                                        last.y + Random.Range(min, max), 
                                                        last.z + Random.Range(min, max));

        BezierPath path = new BezierPath(pathPoints, false, PathSpace.xyz);
        path.FlipNormals = true;
        pathCreator.bezierPath = path;
    }
}

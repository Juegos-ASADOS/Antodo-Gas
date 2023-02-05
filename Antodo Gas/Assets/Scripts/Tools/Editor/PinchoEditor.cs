using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PathCreation;

[CustomEditor(typeof(PinchoTool))]
public class PinchoEditor : Editor
{
    Color handlesStartCol;
    private float offset = 5.0f;

    private void OnSceneGUI()
    {

        //Debug.Log("pinvho");
        var t = (target as PinchoTool);
        //objetivo, rotar y acerca al pincho a la curva igual que le objeto de jugador
        //acercarlo a la curva, al punto y ponerlo con rot 0 encima

       // Debug.Log(t.culva != null);

        PathCreator cula = t.transform.parent.GetComponentInParent<PathCreator>();

        float distanceTravelled = cula.path.GetClosestDistanceAlongPath(t.transform.position);
        //t.angle = cula.path.GetRotationAtDistance()

        //t.transform.Rotate(0, 0, 0); //angulo 0 de momento
        //t.transform.position = t.transform.position + t.transform.up * (sampleToJump.scale.x);

        t.transform.position = cula.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        t.transform.rotation = cula.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        t.transform.Rotate(0, 0, t.angle);
        t.transform.position = t.transform.position + t.transform.up * t.offSet;

    }
}

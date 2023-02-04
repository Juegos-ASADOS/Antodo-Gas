using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RamaExample))]

public class normalSurfaceEditor : Editor
{

    //public void OnSceneGUI()
    //{
    //    var t = target as RamaExample;
    //    var tr = t.transform;


    //    //if (Event.current.type == EventType.MouseDown)
    //    //{
    //    Ray ray = Camera.current.ScreenPointToRay(Event.current.mousePosition);
    //        RaycastHit colision = new RaycastHit();
    //        if (Physics.Raycast(ray, out colision, 1000.0f))
    //        {
    //            Debug.Log(Event.current.mousePosition);
    //            Vector3 position = colision.point;
    //            Vector3 normal = colision.normal;
                
    //            Debug.DrawLine(position, position + normal * 3, Color.red);
    //    }


    //    //}
    //}

}

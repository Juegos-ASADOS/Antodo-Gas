using UnityEngine;
using UnityEditor;

// Displays lines of various thickness in the scene view
[CustomEditor(typeof(ExampleScript))]
public class ExampleEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = target as ExampleScript;
        var tr = t.transform;
        var position = tr.position;
        Handles.color = Color.yellow;
        for (int i = 0; i < 10; ++i)
        {
            var linePos = position + Vector3.right * (i * 0.5f);
            Handles.DrawLine(linePos, linePos + Vector3.up, i);
        }
        //Debug.Log("dibujando linotas");
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
       // Debug.Log("dibujando linotas");

    }
}
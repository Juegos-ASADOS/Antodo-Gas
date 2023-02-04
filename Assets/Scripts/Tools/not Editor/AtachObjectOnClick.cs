
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RamaExample))]

public class AtachObjectOnClick : Editor
{

    public void OnSceneGUI()
    {
        var t = target as RamaExample;
        var o = t.obstaculo;


        //if (Event.current.type == EventType.MouseDown)
        //{
        if (Event.current.type == EventType.MouseDown)
        {
            Ray ray = Camera.current.ScreenPointToRay(Event.current.mousePosition);
            RaycastHit colision = new RaycastHit();
            if (Physics.Raycast(ray, out colision, 1000.0f))
            {
                Debug.Log(Event.current.mousePosition);
                Vector3 position = colision.point;
                Vector3 normal = colision.normal;

                Debug.DrawLine(position, position + normal * 3, Color.red);
                o.transform.position = position;
                Debug.Log(normal);

                o.transform.rotation = Quaternion.LookRotation(normal);

            }
        }


        //}
    }

}

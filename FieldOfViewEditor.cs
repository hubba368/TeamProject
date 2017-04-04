using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    //used to see field of view in scene view
    void OnSceneGUI()
    {
        //generate view radius and view angles in scene view
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        //if target is in view
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            if (visibleTarget == null)
            {
                //if more than one human - needed in case a tree is removed by another human
                return;
            }
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}

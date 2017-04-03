using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Breakability))]
[CanEditMultipleObjects]
public class BreakabilityEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Breakability targetScript = (Breakability) target;

        if (GUILayout.Button("Setup Object"))
        {
            targetScript.SetupGameObjectForShattering();
        }
    }
	
}

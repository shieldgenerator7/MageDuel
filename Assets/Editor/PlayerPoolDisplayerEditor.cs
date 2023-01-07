using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerPoolDisplayer))]
public class PlayerPoolDisplayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerPoolDisplayer ppd = (PlayerPoolDisplayer)target;

        if (GUILayout.Button("Increase health"))
        {
            ppd.Player.heal(1);
        }
        if (GUILayout.Button("Decrease health"))
        {
            ppd.Player.takeDamage(1);
        }
    }

}

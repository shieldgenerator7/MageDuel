using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineupDisplayer))]
public class LineupDisplayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LineupDisplayer ld = (LineupDisplayer)target;

        // foreach(SpellObject spell in ld.testSpells)
        ld.testSpells.ForEach(spell =>
        {
            if (GUILayout.Button($"Add spell {spell.spell.name}"))
            {
                ld.Player.lineupSpell(spell.spell);
            }
        });

    }
}

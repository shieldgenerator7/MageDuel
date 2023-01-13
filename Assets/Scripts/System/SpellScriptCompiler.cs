using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellScriptCompiler
{
    public static List<SpellEffect> compile(string spellScript)
    {
        List<SpellEffect> spellEffects = new List<SpellEffect>();
        string[] lines = spellScript.Trim().Split('\n');
        foreach (string line in lines)
        {
            string lineT = line.Trim();
            if (lineT.StartsWith("damage"))
            {
                spellEffects.Add(new DamageSpellEffect());
            }
            else if (lineT.StartsWith("heal"))
            {
                spellEffects.Add(new HealSpellEffect());
            }
            else if (lineT.StartsWith("block"))
            {
                spellEffects.Add(new BlockSpellEffect());
            }
            else
            {
                Debug.LogError($"Unknown command {lineT} in spell script {spellScript}");
            }
        }
        return spellEffects;
    }
}

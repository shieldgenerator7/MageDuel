using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellScriptCompiler
{
    private static Dictionary<string, Type> spellEffectTypes = new Dictionary<string, Type>(){
        { "damage", typeof(DamageSpellEffect) },
        { "heal", typeof(HealSpellEffect) },
        { "block", typeof(BlockSpellEffect) },
        {"focusRamp",typeof(FocusRamp) },
        {"dodge",typeof(Dodge) },
        {"aura",typeof(AuraAdjust) },
        {"focusSpell",typeof(FocusSpellSpellEffect) },
        {"redirect",typeof(Redirect) },
        {"shiftLineup",typeof(ShiftLineUp) },
        {"undodgableElement",typeof(UndodgableElement) },
        {"breakDefense",typeof(BreakDefense) },
        {"spellSpeed",typeof(SpellSpeedAdjust) },
        {"adjustDamageTaken",typeof(AdjustDamageTaken) },
        {"flipSign",typeof(FlipSign) },
    };

    public static void compile(SpellContext spellContext)
    {
        spellContext.acceptCompile(compile(spellContext.spell.script));
    }

    public static List<ScriptToken> compile(string spellScript)
    {
        List<ScriptToken> scriptTokens = new List<ScriptToken>();
        string[] lines = spellScript.Trim().Split('\n');
        foreach (string line in lines)
        {
            string lineT = line.Trim();
            int firstParen = lineT.IndexOf('(');
            if (firstParen < 0)
            {
                firstParen = lineT.IndexOf(' ');
            }
            string command = (firstParen >= 0) ? lineT.Substring(0, firstParen).Trim() : lineT;
            int secondParen = lineT.IndexOf(')');
            if (secondParen < 0)
            {
                secondParen = lineT.Length - 1;
            }
            string[] args = lineT.Substring(firstParen + 1, secondParen - firstParen - 1).Split(',');
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim();
            }
            scriptTokens.Add(createSpellEffect(command, args));
        }
        return scriptTokens;
    }

    private static SpellEffect createSpellEffect(string command, string[] args)
    {
        if (!spellEffectTypes.ContainsKey(command))
        {
            Debug.LogError($"Unknown command {command}!");
        }
        Type spellEffectType = spellEffectTypes[command];
        SpellEffect spellEffect = (SpellEffect)spellEffectType
            .GetConstructor(new Type[] { })
            .Invoke(new object[] { });
        spellEffect.setArgs(args);
        return spellEffect;
    }
}

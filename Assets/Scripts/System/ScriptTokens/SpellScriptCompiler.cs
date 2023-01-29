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
        {"dispell",typeof(Dispell) },
        {"checkTimeLeft",typeof(CheckTimeLeft) },
        {"damageHealth",typeof(DamageHealth) },
    };
    private static Dictionary<string, Type> delegateRegistrarTypes = new Dictionary<string, Type>()
    {
        {"onTargetedByPlayer", typeof(OnTargetedByPlayer) },
        {"onMatchUpChanged", typeof(OnMatchUpChanged) },
        {"onDamageReceived", typeof(OnDamageReceived) },
    };

    public static void compile(SpellContext spellContext)
    {
        spellContext.acceptCompile(compile(spellContext.spell.script));
    }

    public static List<ScriptToken> compile(string spellScript)
    {
        List<ScriptToken> scriptTokens = new List<ScriptToken>();
        string[] lines = spellScript.Trim().Split('\n');
        int braceLevels = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            //Delegate Registrar Compiling
            if (line.Contains("{"))
            {
                string delegateName = line.Split('{')[0].Trim();
                braceLevels++;
                string text = "";
                for (int j = i + 1; j < lines.Length; j++)
                {
                    string lineJ = lines[j];
                    //begin a sub brace
                    if (lineJ.Contains("{"))
                    {
                        braceLevels++;
                        text += lineJ + "\n";
                    }
                    //end a brace
                    else if (lineJ.Contains("}"))
                    {
                        braceLevels--;
                        //end a sub brace
                        if (braceLevels > 0)
                        {
                            text += lineJ + "\n";
                        }
                        //end this registrar's brace
                        else
                        {
                            scriptTokens.Add(createDelegateRegistrar(
                                delegateName,
                                compile(text)
                                ));
                            i = j;
                            break;
                        }
                    }
                    //add a line of code
                    else
                    {
                        text += lineJ + "\n";
                    }
                }
            }
            else if (line.Contains("}"))
            {
                //do nothing
                Debug.LogError($"Shouldn't be able to process {line} here; it should already be processed");
            }
            //Spell Effect Compiling
            else
            {
                scriptTokens.Add(compileSpellEffect(line));
            }
        }
        return scriptTokens;
    }

    private static SpellEffect compileSpellEffect(string lineT)
    {
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
        return createSpellEffect(command, args);
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

    private static DelegateRegistrar createDelegateRegistrar(string delegateName, List<ScriptToken> tokens)
    {
        if (!delegateRegistrarTypes.ContainsKey(delegateName))
        {
            Debug.LogError($"Unknown delegate registrar {delegateName}!");
        }
        Type delegateType = delegateRegistrarTypes[delegateName];
        DelegateRegistrar delgt = (DelegateRegistrar)delegateType
            .GetConstructor(new Type[] { })
            .Invoke(new object[] { });
        delgt.init(tokens);
        return delgt;
    }


}

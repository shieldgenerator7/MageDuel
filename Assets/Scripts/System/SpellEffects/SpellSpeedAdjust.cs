using System.Collections;
using UnityEngine;

public class SpellSpeedAdjust : SpellEffect, SpellEffectMod
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        //Target a specific spell
        string targetName = getParameter(1);
        if (!string.IsNullOrEmpty(targetName))
        {
            if (!checkTarget(true)) { return; }

            SpellContext target = spellContext.getTarget(targetName);
            target.addMod(this, true);
        }
        //Target all spells of that player
        else
        {
            spellContext.target.Lineup
                .FindAll(spell => !spell.Processed)
                .ForEach(spell => spell.addMod(this, true));
        }
    }

    public int modVariable(string name, int value)
    {
        if (name == "speed")
        {
            return value + Parameter0;
        }
        return value;
    }
}
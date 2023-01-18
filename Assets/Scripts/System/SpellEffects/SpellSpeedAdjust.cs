using System.Collections;
using UnityEngine;

public class SpellSpeedAdjust : SpellEffect, SpellEffectMod
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        spellContext.target.Lineup
            .FindAll(spell => !spell.Processed)
            .ForEach(spell => spell.addMod(this, true));
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
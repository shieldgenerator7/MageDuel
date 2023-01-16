using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }
        SpellContext target = spellContext.getTarget(getParameter(0));
        if (target.target == target.caster)
        {
            target.target = target.caster.opponent;
        }
        else if (target.target == target.caster.opponent)
        {
            target.target = target.caster;
        }
    }
}

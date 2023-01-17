using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }
        SpellContext target = spellContext.getTarget(getParameter(0));
        //Redirect self-targeting spell
        if (target.target == target.caster)
        {
            target.redirect(target.caster.opponent);
        }
        //Redirect opponent-targeting spell
        else if (target.target != null)
        {
            target.redirect(target.caster);
        }
        //Non-targeting spell
        else
        {
            //do nothing
        }
    }
}

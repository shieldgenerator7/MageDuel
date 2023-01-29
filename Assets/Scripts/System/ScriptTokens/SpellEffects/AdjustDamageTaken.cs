using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDamageTaken : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (!checkTarget()) { return; }
        //Set damageAdjust
        string varName = getParameter(0);
        int damage = spellContext.getAttribute(varName);
        int adjust = Parameter1;
        //adjust damage
        damage += adjust;
        if (damage < 0)
        {
            damage = 0;
        }
        spellContext.setAttribute(varName, damage);
    }
}

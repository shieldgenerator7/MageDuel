using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraAdjust : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        int aura = spellContext.getAttribute(getParameter(0));
        spellContext.target.aura.Value += aura;
    }
}

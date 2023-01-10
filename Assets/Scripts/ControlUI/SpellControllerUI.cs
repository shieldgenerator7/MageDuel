using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellControllerUI : PlayerControlUI
{
    public SpellContext spellContext;

    public override void activate()
    {
        spellContext.caster.focusSpell(spellContext, 1);
    }
}

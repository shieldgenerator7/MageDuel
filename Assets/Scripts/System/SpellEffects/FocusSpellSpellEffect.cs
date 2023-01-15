using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSpellSpellEffect : SpellEffect
{
    public override void activate()
    {
        SpellContext spell = spellContext.getTarget(getParameter(0));
        int focusDelta = spellContext.getAttribute(getParameter(1));
        spell.Focus += focusDelta;
    }
}

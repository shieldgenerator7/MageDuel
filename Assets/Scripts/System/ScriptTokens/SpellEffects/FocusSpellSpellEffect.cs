using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSpellSpellEffect : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }
        SpellContext spell = Target0;
        int focusDelta = Parameter1;
        spell.Focus += focusDelta;
    }
}

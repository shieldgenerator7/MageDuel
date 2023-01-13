using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRamp : SpellEffect
{
    public override void activate()
    {
        string argFromRamp = getParameter(0);
        string argToRamp = getParameter(1) ?? argFromRamp;
        int ramped = spellContext.getAttribute(argFromRamp) * spellContext.Focus;
        spellContext.setAttribute(argToRamp, ramped);
    }
}

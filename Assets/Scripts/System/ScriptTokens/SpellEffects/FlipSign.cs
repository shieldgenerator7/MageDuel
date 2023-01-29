using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSign : SpellEffect
{
    public override void activate()
    {
        int var = Parameter0;
        var *= -1;
        spellContext.setAttribute(getParameter(0), var);
    }
}

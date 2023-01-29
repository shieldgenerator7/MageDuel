using System.Collections;
using UnityEngine;

public class CheckTimeLeft : SpellEffect
{
    public override void activate()
    {
        string varName = getParameter(0);
        int timeLeft = spellContext.getAttribute(varName);
        timeLeft--;
        spellContext.setAttribute(varName, timeLeft);
        if (timeLeft <= 0)
        {
            spellContext.dispell();
        }
    }
}
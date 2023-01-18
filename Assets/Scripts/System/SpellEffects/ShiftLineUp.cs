using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftLineUp : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) { return; }
        int shift = spellContext.getAttribute(getParameter(0));
        if (shift < 0)
        {
            //TODO: support negative numbers
            Debug.LogError($"Shift value not yet supported! {getParameter(0)}: {shift}");
            return;
        }
        if (shift > 0)
        {
            //Assumption: shift is 1 or more
            int index = spellContext.caster.game.MatchupIndex;
            for (int i = 0; i < shift; i++)
            {
                spellContext.target.lineupSpell(null, index);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpellEffect : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (!checkTarget()) { return; }
        //Shield target
        spellContext.target.onDamageReceived += onDamageReceived;
    }

    private int onDamageReceived(int damage)
    {
        int defense = spellContext.getAttribute(getParameter(0));
        //Check to see if 
        if (damage > defense)
        {
            //unregister
            spellContext.target.onDamageReceived -= onDamageReceived;
        }
        //block all damage
        return 0;
    }
}

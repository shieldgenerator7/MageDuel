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
        spellContext.target.applyEffect(this, true);
    }

    private int onDamageReceived(int damage)
    {
        int defense = Parameter0;
        //Check to see if it breaks
        if (damage > defense)
        {
            breakDefense();
        }
        //block all damage
        return 0;
    }

    public void breakDefense()
    {
        //unregister
        spellContext.target.onDamageReceived -= onDamageReceived;
        spellContext.target.applyEffect(this, false);
    }
}

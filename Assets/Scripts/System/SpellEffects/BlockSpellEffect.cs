using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpellEffect : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (spellContext.target == null)
        {
            Debug.Log($"Can't shield target {spellContext.target}");
            return;
        }
        //Shield target
        spellContext.target.onDamageReceived += onDamageReceived;
    }

    private int onDamageReceived(int damage)
    {
        //unregister
        spellContext.target.onDamageReceived -= onDamageReceived;
        //block all damage
        return 0;
    }
}

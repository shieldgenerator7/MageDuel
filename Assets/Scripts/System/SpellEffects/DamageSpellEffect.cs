using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (spellContext.target == null)
        {
            Debug.Log($"Can't deal damage to target {spellContext.target}");
            return;
        }
        //Damage target
        int damage = spellContext.getAttribute(getParameter(0));
        Debug.Log($"Damage effect: {damage}, {spellContext.target}");
        spellContext.target.takeDamage(damage);
    }
}

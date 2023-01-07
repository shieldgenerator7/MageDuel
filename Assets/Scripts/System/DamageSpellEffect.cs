using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    public int damage = 1;
    public int damagePerFocus = 0;

    public override void activate(SpellContext context)
    {
        context.target.takeDamage(damage + damagePerFocus * context.focusSpent);
    }
}

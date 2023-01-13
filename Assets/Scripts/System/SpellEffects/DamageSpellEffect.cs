using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    public override void activate()
    {
        int damage = spellContext.getAttribute(getParameter(0));
        Debug.Log($"Damage effect: {damage}, {spellContext.target}");
        spellContext.target.takeDamage(damage);
    }
}

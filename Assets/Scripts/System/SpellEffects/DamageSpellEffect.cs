using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    public override void activate(SpellContext context)
    {
        int damage = context.getAttribute("damage");
        int damagePerFocus = context.getAttribute("damagePerFocus");
        Debug.Log($"Damage effect: {damage}, {damagePerFocus}, {context.target}");
        context.target.takeDamage(damage + damagePerFocus * context.Focus);
    }
}

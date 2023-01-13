using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpellEffect : SpellEffect
{
    public override void activate()
    {
        int damage = spellContext.getAttribute("damage");
        int damagePerFocus = spellContext.getAttribute("damagePerFocus");
        Debug.Log($"Damage effect: {damage}, {damagePerFocus}, {spellContext.target}");
        spellContext.target.takeDamage(damage + damagePerFocus * spellContext.Focus);
    }
}

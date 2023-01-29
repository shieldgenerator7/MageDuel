using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHealth : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) {  return; }

        int damage = Parameter0;
        spellContext.target.health.Value -= damage;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDisplayer : PlayerDisplayUI
{
    SpellContext spellContext;

    public void init(Spell spell, Player caster)
    {
        this.player = caster;
        this.spellContext = new SpellContext(spell, player);
    }

    protected override void _registerDelegates(bool register)
    {
        Debug.LogWarning("Not implemented in SpellDisplayer");
    }

    public override void forceUpdate()
    {
        Debug.LogWarning("Not implemented in SpellDisplayer");
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplayer : PlayerDisplayUI
{
    public Image spellImage;

    SpellContext spellContext;

    public void init(Spell spell, Player caster)
    {
        this.player = caster;
        this.spellContext = new SpellContext(spell, player);
        updateColor();
    }

    protected override void _registerDelegates(bool register)
    {
        Debug.LogWarning("Not implemented in SpellDisplayer");
    }

    public override void forceUpdate()
    {
        Debug.LogWarning("Not implemented in SpellDisplayer");
    }

    private void updateColor()
    {
        spellImage.color = spellContext.spell.element.color;
    }

}

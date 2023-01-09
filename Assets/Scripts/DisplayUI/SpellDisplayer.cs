using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplayer : PlayerDisplayUI
{
    public Image spellImage;

    public SpellContext spellContext;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
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

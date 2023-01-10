using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplayer : PlayerDisplayUI
{
    public List<Sprite> focusSprites;
    public Image imgFocus;
    public Image spellImage;

    public SpellContext spellContext;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
        forceUpdate();
    }

    protected override void _registerDelegates(bool register)
    {
        spellContext.onFocusChanged -= updateFocus;
        if (register)
        {
            spellContext.onFocusChanged += updateFocus;
        }
    }

    public override void forceUpdate()
    {
        updateColor();
        updateFocus(spellContext.Focus);
    }

    private void updateColor()
    {
        spellImage.color = spellContext.element.color;
    }

    private void updateFocus(int focus)
    {
        imgFocus.sprite = focusSprites[focus];
    }

}

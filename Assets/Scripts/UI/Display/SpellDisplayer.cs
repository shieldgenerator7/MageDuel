using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplayer : PlayerDisplayUI
{
    public List<Sprite> focusSprites;
    public List<Sprite> auraSprites;
    public Image imgFocus;
    public Image imgAura;
    public List<Image> imagesToColor;
    //Tooltip
    public ToolTipDisplayer tooltip;

    public SpellContext spellContext;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
        forceUpdate();
    }

    protected override void _registerDelegates(bool register)
    {
        spellContext.onFocusChanged -= updateFocus;
        spellContext.onAuraChanged -= updateAura;
        if (register)
        {
            spellContext.onFocusChanged += updateFocus;
            spellContext.onAuraChanged += updateAura;
        }
    }

    public override void forceUpdate()
    {
        updateColor();
        updateFocus(spellContext.Focus);
        updateAura(spellContext.Aura);
        showTooltip(false);
    }

    private void updateColor()
    {
        Color color = spellContext.element.color;
        imagesToColor.ForEach(img => img.color = color);
    }

    private void updateFocus(int focus)
    {
        imgFocus.sprite = focusSprites[focus];
    }

    private void updateAura(int aura)
    {
        imgAura.sprite = auraSprites[aura];
    }

    public void showTooltip(bool show)
    {
        if (show)
        {
            tooltip.spellContext = this.spellContext;
            tooltip.registerDelegates(player, true);
            tooltip.setUIVars(uiVars);
            tooltip.forceUpdate();
        }
        tooltip.gameObject.SetActive(show);
    }

}

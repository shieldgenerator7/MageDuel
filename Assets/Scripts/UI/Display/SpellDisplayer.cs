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
    //Pulse
    public Image imgPulse;
    //OnResolve
    public List<MonoBehaviour> cmpToDestroyOnResolveList;
    public Color resolveColor = Color.white;

    public SpellContext spellContext;

    public void init(SpellContext spellContext, Player player)
    {
        this.spellContext = spellContext;
        this.registerDelegates(player, true);
        forceUpdate();
    }

    protected override void _registerDelegates(bool register)
    {
        spellContext.onFocusChanged -= updateFocus;
        spellContext.onAuraChanged -= updateAura;
        spellContext.onBinDran -= checkShowPulse;
        spellContext.OnSpellResolved -= onSpellResolved;
        uiVars.game.onPhaseChanged -= onGamePhaseChanged;
        if (register)
        {
            spellContext.onFocusChanged += updateFocus;
            spellContext.onAuraChanged += updateAura;
            spellContext.onBinDran += checkShowPulse;
            spellContext.OnSpellResolved += onSpellResolved;
            uiVars.game.onPhaseChanged += onGamePhaseChanged;
        }
    }

    public override void forceUpdate()
    {
        updateColor();
        updateFocus(spellContext.Focus);
        updateAura(spellContext.Aura);
        showTooltip(false);
        checkShowPulse();
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

    public void showPulse(bool show)
    {
        imgPulse.gameObject.SetActive(show);
    }

    public void checkShowPulse(bool alwaysTrue = true)
    {
        showPulse(
            spellContext.canBeCastNext
            && uiVars.game.Phase == Game.GamePhase.MATCHUP
            );
    }

    public void onGamePhaseChanged(Game.GamePhase phase)
    {
        checkShowPulse();
    }

    public void onSpellResolved(SpellContext spellContext)
    {
        showPulse(false);
        updateFocus(0);
        updateAura(0);
        cmpToDestroyOnResolveList.ForEach(cmp => Destroy(cmp));
        imagesToColor.ForEach(img => img.color = resolveColor);
    }

}

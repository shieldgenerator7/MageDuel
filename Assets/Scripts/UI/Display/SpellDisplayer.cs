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
    //SelectRing
    public Image imgSelectRing;
    //OnResolve
    public List<MonoBehaviour> cmpToDestroyOnResolveList;
    public List<Image> imgToRecolorOnResolve;
    public Color resolveColor = Color.white;
    public List<Image> imgToTransparent;
    [Range(0, 1)]
    public float resolveAlpha = 0.5f;

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
        spellContext.onStateChanged -= checkShowSelectRing;
        spellContext.OnSpellProcessed -= onSpellResolved;
        uiVars.game.onPhaseChanged -= onGamePhaseChanged;
        uiVars.onValidTargetsChanged -= onValidTargetsChanged;
        uiVars.onCurrentCastingSpellChanged -= checkShowPulse;
        if (register)
        {
            spellContext.onFocusChanged += updateFocus;
            spellContext.onAuraChanged += updateAura;
            spellContext.onBinDran += checkShowPulse;
            spellContext.onStateChanged += checkShowSelectRing;
            spellContext.OnSpellProcessed += onSpellResolved;
            uiVars.game.onPhaseChanged += onGamePhaseChanged;
            uiVars.onValidTargetsChanged += onValidTargetsChanged;
            uiVars.onCurrentCastingSpellChanged += checkShowPulse;
        }
    }

    public override void forceUpdate()
    {
        updateColor();
        updateFocus(spellContext.Focus);
        updateAura(spellContext.Aura);
        showTooltip(false);
        checkShowPulse();
        checkShowSelectRing(spellContext.state);
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
            uiVars.game.Phase == Game.GamePhase.MATCHUP
            && ((uiVars.CurrentTargetingSpell != null)
                ? uiVars.ValidTargets?.Contains(spellContext) ?? false
                : spellContext.canBeCastNext)
            );
    }
    public void checkShowPulse(SpellContext spellContext)
    {
        checkShowPulse();
    }

    public void showSelectRing(bool show)
    {
        imgSelectRing.gameObject.SetActive(show);
    }
    public void checkShowSelectRing(SpellContext.State state)
    {
        showSelectRing(state == SpellContext.State.CASTING);
    }

    public void onGamePhaseChanged(Game.GamePhase phase)
    {
        checkShowPulse();
    }

    public void onSpellResolved(SpellContext spellContext)
    {
        showPulse(false);
        cmpToDestroyOnResolveList.ForEach(cmp => Destroy(cmp));
        imgToRecolorOnResolve.ForEach(img => img.color = resolveColor);
        imgToTransparent.ForEach(img =>
        {
            Color c = img.color;
            c.a = resolveAlpha;
            img.color = c;
        });
    }

    public void onValidTargetsChanged(List<Target> targets)
    {
        checkShowPulse();
    }

}

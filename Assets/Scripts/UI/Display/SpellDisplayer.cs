using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplayer : SpellDisplayUI
{
    public Image imgIcon;
    public List<Sprite> focusSprites;
    public List<Sprite> auraSprites;
    public Image imgFocus;
    public Image imgAura;
    public List<Image> imagesToColor;
    //Tooltip
    public PageDisplayerUI tooltip;
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

    public override void init(SpellContext spellContext)
    {
        base.init(spellContext);
        tooltip.setUIVars(uiVars);
        tooltip.init(this.spellContext);
    }
    public override void init(Spell spell)
    {
        base.init(spell);
        tooltip.setUIVars(uiVars);
        tooltip.init(this.spell);
    }

    protected override void _registerDelegates(bool register)
    {
        if (spellContext != null)
        {
            spellContext.onFocusChanged -= updateFocus;
            spellContext.onAuraChanged -= updateAura;
            spellContext.onBinDran -= checkShowPulse;
            spellContext.onStateChanged -= checkShowSelectRing;
            spellContext.OnSpellProcessed -= onSpellResolved;
        }
        uiVars.game.onPhaseChanged -= onGamePhaseChanged;
        uiVars.game.onSubPhaseChanged -= onGameSubPhaseChanged;
        uiVars.onValidTargetsChanged -= onValidTargetsChanged;
        uiVars.onCurrentCastingSpellChanged -= checkShowPulse;
        if (register)
        {
            if (spellContext != null)
            {
                spellContext.onFocusChanged += updateFocus;
                spellContext.onAuraChanged += updateAura;
                spellContext.onBinDran += checkShowPulse;
                spellContext.onStateChanged += checkShowSelectRing;
                spellContext.OnSpellProcessed += onSpellResolved;
            }
            uiVars.game.onPhaseChanged += onGamePhaseChanged;
            uiVars.game.onSubPhaseChanged += onGameSubPhaseChanged;
            uiVars.onValidTargetsChanged += onValidTargetsChanged;
            uiVars.onCurrentCastingSpellChanged += checkShowPulse;
        }
    }

    public override void forceUpdate()
    {
        if (spellContext == null && spell == null)
        {
            Debug.Log($"No spell! {spellContext}, {spell}");
            return;
        }
        imgIcon.sprite = spellContext?.spell.icon ?? spell.icon;
        updateColor();
        updateFocus(spellContext?.Focus ?? -1);
        updateAura(spellContext?.Aura ?? -1);
        showTooltip(false);
        checkShowPulse();
        checkShowSelectRing(spellContext?.state ?? SpellContext.State.LINEDUP);
    }

    private void updateColor()
    {
        Color color = spellContext?.element.color ?? spell.element.color;
        imagesToColor.ForEach(img => img.color = color);
    }

    private void updateFocus(int focus)
    {
        bool show = focus >= 0;
        imgFocus.enabled = show;
        if (show)
        {
            imgFocus.sprite = focusSprites[focus];
        }
    }

    private void updateAura(int aura)
    {
        bool show = aura >= 0;
        imgAura.enabled = show;
        if (show)
        {
            imgAura.sprite = auraSprites[aura];
        }
    }

    public void showTooltip(bool show)
    {
        tooltip.showPage(show);
    }

    public void showPulse(bool show)
    {
        imgPulse.gameObject.SetActive(show);
    }

    public void checkShowPulse(bool alwaysTrue = true)
    {
        showPulse(
            spellContext != null
            && uiVars.game.Phase == Game.GamePhase.MATCHUP
            && ((uiVars.CurrentTargetingSpell != null)
                ? uiVars.ValidTargets?.Contains(spellContext) ?? false
                : uiVars.game.SubPhase == Game.GameSubPhase.CASTING
                    && spellContext.canBeCastNext
                )
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
    public void onGameSubPhaseChanged(Game.GameSubPhase subphase)
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

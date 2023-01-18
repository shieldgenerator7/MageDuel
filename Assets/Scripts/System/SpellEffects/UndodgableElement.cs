using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndodgableElement : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        spellContext.target.onSpellEffectsChanged += searchForDodges;
        spellContext.target.applyEffect(this, true);
        searchForDodges(spellContext.target.SpellEffects);
        spellContext.caster.game.onPhaseChanged += endEffect;
    }

    private void searchForDodges(List<SpellEffect> effects)
    {
        searchForDodges(effects, true);
    }

    private void searchForDodges(List<SpellEffect> effects, bool register)
    {
        spellContext.target.SpellEffects
            .FindAll(effect => effect is Dodge)
            .ForEach(effect => registerUnDodge((Dodge)effect, register));
    }

    private void registerUnDodge(Dodge dodge, bool register)
    {
        dodge.onAboutToDodge -= undodge;
        if (register)
        {
            dodge.onAboutToDodge += undodge;
        }
    }

    private bool undodge(SpellContext otherSpell)
    {
        if (otherSpell.element == this.spellContext.element)
        {
            //cant dodge spells of this spell's element
            return false;
        }
        //can dodge this spell
        return true;
    }

    private void endEffect(Game.GamePhase phase)
    {
        if (phase == Game.GamePhase.CLEANUP)
        {
            searchForDodges(spellContext.target.SpellEffects, false);
            spellContext.target.onSpellEffectsChanged -= searchForDodges;
            spellContext.target.applyEffect(this, false);
            spellContext.caster.game.onPhaseChanged -= endEffect;
        }
    }
}

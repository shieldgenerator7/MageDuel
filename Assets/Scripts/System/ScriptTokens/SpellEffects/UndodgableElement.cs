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
        searchForDodges(spellContext.target.ScriptTokens);
        spellContext.caster.game.onPhaseChanged += endEffect;
    }

    private void searchForDodges(List<ScriptToken> effects)
    {
        searchForDodges(effects, true);
    }

    private void searchForDodges(List<ScriptToken> effects, bool register)
    {
        spellContext.target.ScriptTokens
            .FindAll(effect => effect.isType<Dodge>())
            .ForEach(effect => registerUnDodge(effect.asType<Dodge>(), register));
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
            searchForDodges(spellContext.target.ScriptTokens, false);
            spellContext.target.onSpellEffectsChanged -= searchForDodges;
            spellContext.target.applyEffect(this, false);
            spellContext.caster.game.onPhaseChanged -= endEffect;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellControllerUI : PlayerControlUI
{
    public SpellContext spellContext;

    private bool rightClick = false;
    private bool shiftKey = false;

    public override void activate()
    {
        switch (uiVars.game.Phase)
        {
            case Game.GamePhase.READYUP:
                break;
            case Game.GamePhase.LINEUP:
                if (player.State == Player.PlayState.FOCUSING)
                {
                    if (rightClick && !shiftKey && spellContext.Focus == 0)
                    {
                        //Refund aura spent on it
                        if (spellContext.Aura > 0)
                        {
                            spellContext.caster.focusSpell(spellContext, 0, spellContext.Aura, false);
                        }
                        //Remove spell from lineup
                        player.removeSpellFromLineup(spellContext);
                    }
                    else
                    {
                        int focus = (!shiftKey) ? 1 : 0;
                        int aura = (shiftKey) ? 1 : 0;
                        spellContext.caster.focusSpell(spellContext, focus, aura, !rightClick);
                    }
                }
                break;
            case Game.GamePhase.MATCHUP:
                //Early exit: another spell is targeting
                if (uiVars.CurrentCastingSpell != null)
                {
                    if (uiVars.ValidTargets.Contains(spellContext))
                    {
                        uiVars.CurrentCastingSpell.acceptTargetAsNext(spellContext);
                    }
                    break;
                }
                //if this is the next spell in the lineup,
                if (spellContext.canBeCastNext)
                {
                    //process it
                    if (!rightClick)
                    {
                        processSpell();
                    }
                    else
                    {
                        spellContext.fizzle();
                    }
                }
                break;
            case Game.GamePhase.CLEANUP:
                break;
            default:
                Debug.LogError($"Unknown game phase! phase: {uiVars.game.Phase}");
                break;
        }
    }

    public override void activate(PointerEventData eventData)
    {
        rightClick = eventData.button == PointerEventData.InputButton.Right;
        shiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        activate();
    }

    private void processSpell()
    {
        //Early exit: not enough focus
        if (!spellContext.FocusPaid)
        {
            spellContext.fizzle();
            return;
        }
        //Activate with targets
        if (spellContext.hasAllTargets())
        {
            spellContext.activate();
        }
        //Wait for target selection
        else
        {
            StartCoroutine(waitForUserTarget());
        }
    }

    private IEnumerator waitForUserTarget()
    {
        uiVars.CurrentCastingSpell = spellContext;
        foreach (SpellTarget target in spellContext.spell.spellTargets)
        {
            uiVars.ValidTargets = spellContext.target.Lineup
                .FindAll(spell =>
                    //Only spells yet to be cast
                    !spell.Resolved
                    //Only spells that arent being targeted, or if targeting the same spell twice is allowed
                    && (!target.requireUnique || !spellContext.isTargetingSpell(spell))
                )
                .ConvertAll(spell => (Target)spell);
            while (!spellContext.hasTarget(target.name))
            {
                yield return target;
            }
        }
        uiVars.ValidTargets.Clear();
        uiVars.CurrentCastingSpell = null;
        spellContext.activate();
    }
}

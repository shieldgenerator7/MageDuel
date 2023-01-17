using System;
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
                if (uiVars.CurrentTargetingSpell != null)
                {
                    if (uiVars.ValidTargets.Contains(spellContext))
                    {
                        uiVars.CurrentTargetingSpell.acceptTargetAsNext(spellContext);
                    }
                    break;
                }
                //if this is the next spell in the lineup (or has Flash),
                if (spellContext.canBeCastNext)
                {
                    //add it to process queue
                    if (!rightClick)
                    {
                        Action enqueueFunc = () => 
                            uiVars.game.queueSpell(spellContext, true);

                        if (spellContext.hasAllTargets())
                        {
                            //Add to queue
                            enqueueFunc();
                        }
                        else { 
                            //Select targets then add to queue
                            StartCoroutine(waitForUserTarget(enqueueFunc));
                        }
                    }
                    else
                    {
                        //Dequeue it
                        if (spellContext.state == SpellContext.State.CASTING)
                        {
                            uiVars.game.queueSpell(spellContext, false);
                        }
                        //fizzle it
                        else
                        {
                            spellContext.fizzle();
                        }
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

    private IEnumerator waitForUserTarget(Action thenFunc)
    {
        uiVars.CurrentTargetingSpell = spellContext;
        foreach (SpellTarget target in spellContext.spell.spellTargets)
        {
            uiVars.ValidTargets = spellContext.target.Lineup
                .FindAll(spell => spell != null
                    //Only spells yet to be cast
                    && !spell.Processed
                    //Only spells that arent being targeted, or if targeting the same spell twice is allowed
                    && (!target.requireUnique || !spellContext.isTargetingSpell(spell))
                )
                .ConvertAll(spell => (Target)spell);
            if (uiVars.ValidTargets.Count == 0)
            {
                continue;
            }
            while (!spellContext.hasTarget(target.name))
            {
                yield return target;
            }
        }
        uiVars.ValidTargets.Clear();
        uiVars.CurrentTargetingSpell = null;
        thenFunc();
    }
}

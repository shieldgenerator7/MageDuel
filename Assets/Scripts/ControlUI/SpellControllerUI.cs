using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellControllerUI : PlayerControlUI
{
    public SpellContext spellContext;

    private bool rightClick = false;

    public override void activate()
    {
        switch (player.State)
        {
            case Player.PlayState.READYING:
            case Player.PlayState.FOCUSING:
                if (rightClick && spellContext.Focus == 0)
                {
                    player.removeSpellFromLineup(spellContext);
                }
                else
                {
                    spellContext.caster.focusSpell(spellContext, 1, 0, !rightClick);
                }                
                break;
            case Player.PlayState.CASTING:
                if (!rightClick)
                {
                    spellContext.activate();
                }
                else
                {
                    player.removeSpellFromLineup(spellContext);
                }
                break;
            default:
                Debug.LogError($"Unknown state! {player.State}");
                break;
        }
    }

    public override void activate(PointerEventData eventData)
    {
        rightClick = eventData.button == PointerEventData.InputButton.Right;
        activate();
    }
}

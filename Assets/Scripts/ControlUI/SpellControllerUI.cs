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
        switch (player.State)
        {
            case Player.PlayState.READYING:
            case Player.PlayState.FOCUSING:
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
        shiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        activate();
    }
}

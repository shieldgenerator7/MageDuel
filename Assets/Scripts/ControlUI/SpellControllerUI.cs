using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellControllerUI : PlayerControlUI
{
    public SpellContext spellContext;

    public override void activate()
    {
        switch (player.State)
        {
            case Player.PlayState.READYING:
            case Player.PlayState.FOCUSING:
                spellContext.caster.focusSpell(spellContext, 1);
                break;
            case Player.PlayState.CASTING:
                spellContext.activate();
                break;
            default:
                Debug.LogError($"Unknown state! {player.State}");
                break;
        }
    }
}

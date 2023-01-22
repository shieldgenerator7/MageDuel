using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageControllerUI : PlayerControlUI
{
    private Spell spell;

    public void init(Spell spell)
    {
        this.spell = spell;
    }

    public override void activate()
    {
        if (uiVars.game.Phase == Game.GamePhase.LINEUP && player.State == Player.PlayState.FOCUSING)
        {
            player.lineupSpell(spell);
        }
    }
}

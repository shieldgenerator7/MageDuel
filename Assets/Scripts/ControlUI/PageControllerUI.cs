using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageControllerUI : PlayerControlUI
{
    public Spell spell;

    public override void activate()
    {
        if (game.Phase == Game.GamePhase.LINEUP && player.State == Player.PlayState.FOCUSING)
        {
            player.lineupSpell(spell);
        }
    }
}

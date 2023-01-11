using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : PlayerControlUI
{
    public override void activate()
    {
        if (uiVars.game.Phase == Game.GamePhase.LINEUP && player.State == Player.PlayState.FOCUSING)
        {
            player.lineupSpell(
                player.deck[uiVars.getPlayerVariables(player).SpellBookPage]
                );
        }
    }

    public void turnPage(int dir)
    {
        if (Mathf.Abs(dir) != 1)
        {
            Debug.LogError($"dir must be -1 or 1! dir: {dir}");
            return;
        }
        uiVars.getPlayerVariables(player).SpellBookPage += dir;
    }
}

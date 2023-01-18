using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHoodController : PlayerControlUI
{
    public override void activate()
    {
        if (uiVars.game.Phase == Game.GamePhase.LINEUP)
        {
            if (player.Lineup.Count > 0)
            {
                player.State = Player.PlayState.CASTING;
            }
        }
        //TEST CODE: rerandomize player deck
        if (uiVars.game.roundNumber <= 1 && player.Lineup.Count == 0)
        {
            List<Deck> decks = uiVars.gameSettings.decks;
            player.Deck = decks[Random.Range(0, decks.Count)];
        }
    }
}

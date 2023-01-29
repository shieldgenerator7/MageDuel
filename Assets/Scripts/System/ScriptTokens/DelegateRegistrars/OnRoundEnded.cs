using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoundEnded : DelegateRegistrar
{
    protected override void registerDelegates(bool register)
    {
        spellContext.target.game.onPhaseChanged -= onRoundEnded;
        if (register)
        {
            spellContext.target.game.onPhaseChanged += onRoundEnded;
        }
    }

    private void onRoundEnded(Game.GamePhase phase)
    {
        if (phase == Game.GamePhase.CLEANUP)
        {
            processTokens();
        }
    }
}

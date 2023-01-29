using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMatchUpChanged : DelegateRegistrar
{
    protected override void registerDelegates(bool register)
    {
        spellContext.target.game.onMatchUpChanged -= onMatchUpChanged;
        if (register)
        {
            spellContext.target.game.onMatchUpChanged += onMatchUpChanged;
        }
    }

    private void onMatchUpChanged(int index) {
        spellContext.setAttribute("onMatchUpChanged", index);
        processTokens();
    }
}

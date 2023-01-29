using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTargetedByPlayer : DelegateRegistrar
{
    protected override void registerDelegates(bool register)
    {
        spellContext.target.onTargetedByPlayer -= onTargetedByPlayer;
        if (register)
        {
            spellContext.target.onTargetedByPlayer += onTargetedByPlayer;
        }
    }

    private void onTargetedByPlayer(Player player, SpellContext spellContext)
    {
        this.spellContext.setTarget("onTargetedByPlayer", spellContext);
        processTokens();
    }
}

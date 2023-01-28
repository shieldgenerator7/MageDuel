using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTargetedByPlayer : DelegateRegistrar
{
    protected override void registerDelegates(bool register)
    {
        spellContext.caster.onTargetedByPlayer -= onTargetedByPlayer;
        if (register)
        {
            spellContext.caster.onTargetedByPlayer += onTargetedByPlayer;
        }
        spellContext.caster.applyEffect(this, register);
    }

    private void onTargetedByPlayer(Player player, SpellContext spellContext)
    {
        spellContext.setTarget("onTargetedByPlayer", spellContext);
        foreach(ScriptToken token in scriptTokens)
        {
            token.init(this.spellContext);
            token.evaluate();
        }
    }
}

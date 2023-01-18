using System.Collections;
using System.Collections.Generic;

public class Dodge : SpellEffect
{
    public override void activate()
    {
        spellContext.caster.onTargetedByPlayer += onTargetedByPlayer;
        spellContext.caster.game.onMatchUpChanged += stopDodging;
        spellContext.caster.applyEffect(this, true);
    }

    private void onTargetedByPlayer(Player player, SpellContext otherSpell)
    {
        //Early exit
        if (player == this.spellContext.caster)
        {
            //don't dodge self-casted spells
            return;
        }
        //Dodge the spell
        otherSpell.target = null;
    }

    private void stopDodging(int index)
    {
        //unregister
        spellContext.caster.onTargetedByPlayer -= onTargetedByPlayer;
        spellContext.caster.game.onMatchUpChanged -= stopDodging;
        spellContext.caster.applyEffect(this, false);
    }
}

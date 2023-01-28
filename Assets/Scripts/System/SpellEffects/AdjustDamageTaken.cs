using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDamageTaken : SpellEffect
{
    private int damageAdjust;
    private int duration;
    private int matchupsLeft;

    public override void activate()
    {
        //Early exit: no target
        if (!checkTarget()) { return; }
        //Set damageAdjust
        damageAdjust = Parameter0;
        duration = Parameter1;
        if (duration == 0)
        {
            Debug.LogError($"No duration set! (parameter1) {duration}");
        }
        matchupsLeft = duration;
        //Shield target
        spellContext.target.onDamageReceived += onDamageReceived;
        spellContext.target.applyEffect(this, true);
        //Register matchup delegate
        spellContext.target.game.onMatchUpChanged += onMatchUpEnded;
    }

    private int onDamageReceived(int damage)
    {
        //adjust damage
        damage += damageAdjust;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }

    public void breakDefense()
    {
        //unregister
        spellContext.target.onDamageReceived -= onDamageReceived;
        spellContext.target.applyEffect(this, false);
        spellContext.target.game.onMatchUpChanged -= onMatchUpEnded;
    }

    private void onMatchUpEnded(int index)
    {
        matchupsLeft--;
        if (matchupsLeft <= 0)
        {
            breakDefense();
        }
    }
}

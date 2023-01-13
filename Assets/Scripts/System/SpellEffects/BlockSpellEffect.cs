using System.Collections;
using System.Collections.Generic;

public class BlockSpellEffect : SpellEffect
{
    public override void activate()
    {
        spellContext.target.onDamageReceived += onDamageReceived;
    }

    private int onDamageReceived(int damage)
    {
        //unregister
        spellContext.target.onDamageReceived -= onDamageReceived;
        //block all damage
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDefense : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        BlockSpellEffect block = spellContext.target.ScriptTokens
            .Find(effect => effect.isType<BlockSpellEffect>())
            ?.asType<BlockSpellEffect>();
        block?.breakDefense();
        if (block == null)
        {
            AdjustDamageTaken adt = spellContext.target.ScriptTokens
                .Find(effect => effect.isType<AdjustDamageTaken>())
                ?.asType<AdjustDamageTaken>();
            adt?.dispell();
        }
    }
}

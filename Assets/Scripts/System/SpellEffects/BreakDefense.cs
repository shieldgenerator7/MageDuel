using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDefense : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget()) { return; }

        BlockSpellEffect block = (BlockSpellEffect)spellContext.target.SpellEffects
            .Find(effect => effect is BlockSpellEffect);
        block?.breakDefense();
        if (block == null)
        {
            AdjustDamageTaken adt = (AdjustDamageTaken)spellContext.target.SpellEffects
                .Find(effect => effect is AdjustDamageTaken);
            adt?.breakDefense();
        }
    }
}

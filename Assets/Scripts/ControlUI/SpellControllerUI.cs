using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellControllerUI : PlayerControlUI
{
    public SpellContext spellContext;

    public override void activate()
    {
        spellContext.spell.effects = SpellScriptCompiler.compile(spellContext.spell.script);
        spellContext.spell.effects.ForEach(effect =>
        {
            effect.activate(spellContext);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellDisplayUI : PlayerDisplayUI
{
    protected SpellContext spellContext;
    protected Spell spell;

    public SpellContext SpellContext => spellContext;

    public virtual void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
        this.spell = spellContext.spell;
        this.registerDelegates(spellContext.caster, true);
        forceUpdate();
    }

    public virtual void init(Spell spell)
    {
        this.spellContext = null;
        this.spell = spell;
    }

    protected bool Revealed => uiVars?.viewPlayer.canView(spellContext) ?? false;
}

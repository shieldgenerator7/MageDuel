using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptToken
{
    protected SpellContext spellContext;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
    }

    public abstract void evaluate();
}

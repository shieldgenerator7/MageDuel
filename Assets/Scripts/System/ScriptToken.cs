using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptToken
{
    public SpellContext spellContext { get; private set; }

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
    }

    public abstract void evaluate();

    public virtual void dispell()
    {
        Debug.LogError($"dispell not implemented! token: {this}");
    }

    public virtual bool isType<T>()
    {
        return this is T;
    }

    public virtual T asType<T>() where T : ScriptToken
    {
        return (T)this;
    }
}

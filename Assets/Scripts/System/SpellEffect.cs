using System;
using UnityEngine;

public abstract class SpellEffect
{
    protected SpellContext spellContext;
    private string[] parameters;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
    }
    internal void setArgs(string[] args)
    {
        this.parameters = args;
    }

    protected string getParameter(int index)
    {
        //early exit
        if (parameters == null)
        {
            return null;
        }
        if (index < 0 || index >= parameters.Length)
        {
            return null;
        }
        //return parameter at index
        return parameters[index];
    }

    public abstract void activate();

    protected bool checkTarget(bool checkSpellTargets = false)
    {
        //Fail: player target null
        if (spellContext.target == null)
        {
            Debug.Log($"Cannot target player {spellContext.target}");
            return false;
        }
        //Fail: spell target(s) null
        if (checkSpellTargets)
        {
            foreach (SpellTarget spellTarget in spellContext.spell.spellTargets)
            {
                SpellContext target = spellContext.getTarget(spellTarget.name);
                if (target == null)
                {
                    Debug.Log($"Cannot target player ({spellContext.target}) spell {spellTarget.name}: {target}");
                    return false;
                }
            }
        }
        //Success
        return true;
    }

}
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
    public void setArgs(string[] args)
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

    protected int Parameter0 => spellContext.getAttribute(getParameter(0));
    protected int Parameter1 => spellContext.getAttribute(getParameter(1));

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
                if (target.Processed)
                {
                    Debug.Log($"Cannot target player ({spellContext.target}) spell {spellTarget.name}: {target}. It has already been processed.");
                    return false;
                }
            }
        }
        //Success
        return true;
    }

}
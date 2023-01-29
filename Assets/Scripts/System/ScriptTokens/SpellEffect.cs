using System;
using UnityEngine;

public abstract class SpellEffect : ScriptToken
{
    private string[] parameters;

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

    protected SpellContext Target0 => spellContext.getTarget(getParameter(0));
    protected SpellContext Target1 => spellContext.getTarget(getParameter(1));

    public override void evaluate()
    {
        activate();
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
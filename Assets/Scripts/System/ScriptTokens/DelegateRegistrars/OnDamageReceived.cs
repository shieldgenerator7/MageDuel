using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageReceived : DelegateRegistrar
{
    protected override void registerDelegates(bool register)
    {
        spellContext.target.onDamageReceived -= onDamageReceived;
        if (register)
        {
            spellContext.target.onDamageReceived += onDamageReceived;
        }
    }

    private int onDamageReceived(int damage)
    {
        string attrName = "onDamageReceived";
        spellContext.setAttribute(attrName, damage);
        processTokens();
        return spellContext.getAttribute(attrName);
    }
}

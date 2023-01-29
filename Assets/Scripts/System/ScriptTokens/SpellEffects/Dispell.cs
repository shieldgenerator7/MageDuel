using System.Collections;
using UnityEngine;

public class Dispell : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }

        SpellContext target = Target0;
        foreach(ScriptToken token in target.target.ScriptTokens)
        {
            if (token.spellContext == target)
            {
                if (token is DelegateRegistrar)
                {
                    ((DelegateRegistrar)token).dispell();
                }
                else
                {
                    Debug.LogError($"Token is not a DelegateRegistrar! token: {token}");
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DelegateRegistrar : ScriptToken
{
    protected List<ScriptToken> scriptTokens { get; private set; } = new List<ScriptToken>();

    public void init(List<ScriptToken> scriptTokens)
    {
        this.scriptTokens = scriptTokens;
    }

    public override void evaluate()
    {
        registerDelegates(true);
        spellContext.caster.game.onMatchUpChanged += onMatchupChanged;
    }

    protected abstract void registerDelegates(bool register);

    //TEST CODE: make a system (or another delegate registrar) to make it unregister more flexibly
    private void onMatchupChanged(int index)
    {
        registerDelegates(false);
        spellContext.caster.game.onMatchUpChanged -= onMatchupChanged;
    }

    public override bool isType<T>()
    {
        return base.isType<T>() || scriptTokens.Any(token => token.isType<T>());
    }

    public override T asType<T>()
    {
        if (base.isType<T>())
        {
            return base.asType<T>();
        }
        return scriptTokens.First(token => token.isType<T>()).asType<T>();
    }
}

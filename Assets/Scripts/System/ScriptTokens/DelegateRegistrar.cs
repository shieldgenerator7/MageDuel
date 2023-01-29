using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DelegateRegistrar : ScriptToken
{
    private List<ScriptToken> scriptTokens = new List<ScriptToken>();

    public void init(List<ScriptToken> scriptTokens)
    {
        this.scriptTokens = scriptTokens;
    }

    public override void evaluate()
    {
        registerDelegates(true);
        spellContext.target.applyEffect(this, true);
    }

    protected abstract void registerDelegates(bool register);

    protected void processTokens()
    {
        foreach (ScriptToken token in scriptTokens)
        {
            token.init(this.spellContext);
            token.evaluate();
        }
    }

    public void dispell()
    {
        registerDelegates(false);
        spellContext.target.applyEffect(this, false);
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

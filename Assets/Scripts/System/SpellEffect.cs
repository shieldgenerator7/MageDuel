using System;

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

}
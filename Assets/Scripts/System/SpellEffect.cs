public abstract class SpellEffect
{
    protected SpellContext spellContext;

    public SpellEffect(SpellContext spellContext)
    {
        this.spellContext = spellContext;
    }

    public abstract void activate();
}
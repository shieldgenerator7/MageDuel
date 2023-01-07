public class SpellContext
{
    public Spell spell;
    public Player target;
    public Player caster;
    public int focusSpent;
    public int auraSpent;

    public int getAttribute(string attrName)
    {
        int value = spell.getAttribute(attrName);
        //TODO: modifications from other effects
        return value;
    }
}
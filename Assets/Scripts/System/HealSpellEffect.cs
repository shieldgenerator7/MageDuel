
public class HealSpellEffect : SpellEffect
{
    public int heal = 0;
    public int healPerFocus = 1;

    public override void activate(SpellContext context)
    {
        context.target.heal(heal + healPerFocus * context.focusSpent);
    }
}

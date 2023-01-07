
public class HealSpellEffect : SpellEffect
{
    public override void activate(SpellContext context)
    {
        int heal = context.getAttribute("heal");
        int healPerFocus = context.getAttribute("healPerFocus");
        context.target.heal(heal + healPerFocus * context.focusSpent);
    }
}

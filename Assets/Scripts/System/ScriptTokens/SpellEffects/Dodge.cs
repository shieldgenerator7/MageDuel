using System.Collections;
using System.Collections.Generic;

public class Dodge : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }

        SpellContext otherSpell = Target0;
        //Early exit
        if (otherSpell.caster == this.spellContext.caster)
        {
            //don't dodge self-casted spells
            return;
        }
        //Early exit: delegate says no (all must say yes to allow dodge)
        if (onAboutToDodge != null)
        {
            bool allowDodge = true;
            foreach (OnAboutToDodge delgt in onAboutToDodge.GetInvocationList())
            {
                if (!delgt(otherSpell))
                {
                    allowDodge = false;
                    //dont just return here bc you might want to process all the delegates
                }
            }
            if (!allowDodge)
            {
                return;
            }
        }
        //Dodge the spell
        otherSpell.target = null;
    }
    public delegate bool OnAboutToDodge(SpellContext otherSpell);
    public event OnAboutToDodge onAboutToDodge;
}

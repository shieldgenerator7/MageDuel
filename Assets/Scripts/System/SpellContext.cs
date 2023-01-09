

using UnityEngine;

public class SpellContext
{
    public Spell spell;
    public Player target;
    public Player caster;
    public int focusSpent;
    public int auraSpent;

    public SpellContext(Spell spell, Player caster)
    {
        this.spell = spell;
        this.caster = caster;
        if (spell.autoTargetEnemy)
        {
            this.target = caster.opponent;
        }
        else if (spell.autoTargetSelf)
        {
            this.target = caster;
        }
        this.focusSpent = 1;//test code
    }

    public int getAttribute(string attrName)
    {
        int value = spell.getAttribute(attrName);
        //TODO: modifications from other effects
        return value;
    }
}
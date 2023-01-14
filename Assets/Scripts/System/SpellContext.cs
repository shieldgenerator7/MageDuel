

using System.Collections.Generic;
using UnityEngine;

public class SpellContext
{
    public Spell spell;
    public Player target;
    public Player caster;
    private int focusSpent;
    private int auraSpent;

    private AttributeSet variables;

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
        variables = new AttributeSet(spell.attributes);
    }

    public Spell Spell => spell;

    public int Focus
    {
        get => focusSpent;
        set
        {
            focusSpent = value;
            onFocusChanged?.Invoke(focusSpent);
        }
    }
    public int Aura
    {
        get => auraSpent;
        set
        {
            auraSpent = value;
            onAuraChanged?.Invoke(auraSpent);
        }
    }
    public delegate void OnChargeChanged(int value);
    public event OnChargeChanged onFocusChanged;
    public event OnChargeChanged onAuraChanged;

    public bool FocusPaid => focusSpent >= spell.cost;

    public Element element => spell.element;

    public string Description
    {
        get
        {
            string desc = this.spell.description;
            //Find vars in desc
            List<string> vars = new List<string>();
            int startBrak = desc.IndexOf('[');
            while (startBrak > 0)
            {
                int endBrak = desc.IndexOf(']', startBrak);
                vars.Add(desc.Substring(startBrak + 1, endBrak - startBrak - 1));
                startBrak = desc.IndexOf('[', endBrak);
            }
            foreach (string attrName in vars)
            {
                desc = desc.Replace($"[{attrName}]", $"{getAttribute(attrName)}");
            }
            return desc;
        }
    }

    public int getAttribute(string attrName)
    {
        SpellAttribute attr = variables.getAttribute(attrName);
        int value = attr.value;
        if (attr.rampable)
        {
            value += auraSpent;
        }
        //TODO: modifications from other effects
        return value;
    }
    public void setAttribute(string attrName, int value)
    {
        variables.set(attrName, value);
    }

    public bool canBeCastNext
        => caster.Lineup.IndexOf(this) == 0
        || spell.keywords.Contains(Spell.Keyword.FLASH);

    public void activate()
    {
        //If focus cost has been paid,
        if (FocusPaid)
        {
            //Target player
            if (target)
            {
                caster.targetPlayer(target, this);
            }
            //Activate spell
            List<SpellEffect> effects = SpellScriptCompiler.compile(spell.script);
            effects.ForEach(effect =>
            {
                effect.init(this);
                effect.activate();
            });
        }
        //Mark this spell resolved regardless
        OnSpellResolved?.Invoke(this);
    }
    public delegate void OnSpell(SpellContext sc);
    public event OnSpell OnSpellResolved;
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    public string name;
    public Pool focus = new Pool(7);
    /// <summary>
    /// The spells that the player knows
    /// </summary>
    public List<Spell> spellList = new List<Spell>();
    /// <summary>
    /// The spells that the player has in the lineup
    /// </summary>
    private List<SpellContext> lineup = new List<SpellContext>();

    public Player opponent;


    public Player(string name) : base(3, 5)
    {
        this.name = name;
    }

    public void focusSpell(SpellContext spellContext, int focus, int aura = 0)
    {
        if (focus < 0)
        {
            Debug.LogError($"Focus must be 0 or greater! focus: {focus}");
            return;
        }
        if (aura < 0)
        {
            Debug.LogError($"Aura must be 0 or greater! aura: {aura}");
            return;
        }
        if (this.focus >= focus)
        {
            this.focus.Value -= focus;
            spellContext.focusSpent += focus;
        }
        if (this.aura >= aura)
        {
            this.aura.Value -= aura;
            spellContext.auraSpent += aura;
        }
    }

    public void lineupSpell(Spell spell)
    {
        SpellContext context = new SpellContext(spell, this);
        lineup.Add(context);
        context.OnSpellResolved += removeSpellFromLineup;
        onLineupChanged?.Invoke(lineup);
    }
    public delegate void OnLineupChanged(List<SpellContext> spellList);
    public event OnLineupChanged onLineupChanged;

    public List<SpellContext> Lineup => lineup.ToList();

    private void removeSpellFromLineup(SpellContext sc)
    {
        lineup.Remove(sc);
        onLineupChanged?.Invoke(lineup);
    }

    public static implicit operator bool(Player player) => player != null;
}

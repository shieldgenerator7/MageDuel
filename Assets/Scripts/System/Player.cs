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

    public void lineupSpell(Spell spell)
    {
        SpellContext context = new SpellContext(spell, this);
        lineup.Add(context);
        onLineupChanged?.Invoke(lineup);
    }
    public delegate void OnLineupChanged(List<SpellContext> spellList);
    public event OnLineupChanged onLineupChanged;

    public List<SpellContext> Lineup => lineup.ToList();

    public static implicit operator bool(Player player) => player != null;
}

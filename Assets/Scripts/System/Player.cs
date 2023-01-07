using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public string name;
    public Pool focus = new Pool(7);
    public List<Spell> spellList = new List<Spell>();

    public Player(string name) : base(3, 5)
    {
        this.name = name;
    }
}

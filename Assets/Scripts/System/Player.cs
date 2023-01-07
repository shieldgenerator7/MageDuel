using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public Pool health = new Pool(3);
    public Pool aura = new Pool(5);
    public Pool focus = new Pool(7);
    public List<Spell> spellList = new List<Spell>();
}

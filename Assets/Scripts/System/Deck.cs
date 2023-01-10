using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public new string name;
    public List<Spell> spellList;

    public List<Element> Elements
        => spellList.ConvertAll(spell => spell.element);

    public Spell this[int i] => spellList[i];
}

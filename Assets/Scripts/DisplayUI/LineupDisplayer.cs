using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupDisplayer : PlayerDisplayUI
{
    public int buffer = 250;

    public GameObject spellPrefab;

    private List<SpellDisplayer> spellObjects = new List<SpellDisplayer>();

    protected override void _registerDelegates(bool register)
    {
        player.onLineupChanged -= layoutSpells;
        if (register)
        {
            player.onLineupChanged += layoutSpells;
        }
        spellObjects.ForEach(so => so.registerDelegates(player, register));
    }

    public override void forceUpdate()
    {
        layoutSpells(player.Lineup);
        spellObjects.ForEach(so => so.forceUpdate());
    }

    private void layoutSpells(List<Spell> spells)
    {
        //TODO: rewrite so it reuses gameobjects
        //
        spellObjects.ForEach(so => Destroy(so.gameObject));
        spellObjects.Clear();
        //
        //Create the spell objects
        foreach (Spell spell in spells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellObjects.Add(spellDisplayer);
            spellDisplayer.init(spell);
        }
        //Arrange the spell objects
        int x = -1 * (spellObjects.Count - 1) * buffer / 2;
        foreach (SpellDisplayer so in spellObjects)
        {
            so.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
            x += buffer;
        }
    }
}

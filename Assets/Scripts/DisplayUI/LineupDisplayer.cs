using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupDisplayer : PlayerDisplayUI
{
    public int buffer = 250;

    public GameObject spellPrefab;

    private List<SpellDisplayer> spellObjects = new List<SpellDisplayer>();

    public List<SpellObject> testSpells;

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
        spellObjects.ForEach(so => Destroy(so.gameObject));
        spellObjects.Clear();
        //
        int x = 0;
        foreach (Spell spell in spells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellObjects.Add(spellDisplayer);
            spellDisplayer.init(spell, player);
            spellObject.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
            x += buffer;
        }
    }
}

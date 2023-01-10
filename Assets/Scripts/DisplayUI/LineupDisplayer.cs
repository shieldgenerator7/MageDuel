using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupDisplayer : PlayerDisplayUI
{
    public int buffer = 250;
    public bool flipped = false;

    public GameObject mageHoodCoin;

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

    private void layoutSpells(List<SpellContext> spells)
    {
        //TODO: rewrite so it reuses gameobjects
        //
        spellObjects.ForEach(so =>
        {
            callOnDisplayerDestroyed(so);
            Destroy(so.gameObject);
        });
        spellObjects.Clear();
        //
        //Create the spell objects
        foreach (SpellContext spell in spells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellObjects.Add(spellDisplayer);
            spellDisplayer.init(spell);
            callOnDisplayerCreated(spellDisplayer);
        }
        //Arrange the spell objects
        int flip = (flipped) ? -1 : 1;
        int x = flip * -1 * (spellObjects.Count) * buffer / 2;
        mageHoodCoin.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
        x += flip * buffer;
        foreach (SpellDisplayer so in spellObjects)
        {
            so.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
            x += flip * buffer;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupDisplayer : PlayerDisplayUI
{
    public int buffer = 250;
    public bool flipped = false;

    public GameObject mageHoodCoin;

    public GameObject spellPrefab;

    private Dictionary<SpellContext, SpellDisplayer> spellGOMap = new Dictionary<SpellContext, SpellDisplayer>();

    protected override void _registerDelegates(bool register)
    {
        player.onLineupChanged -= layoutSpells;
        if (register)
        {
            player.onLineupChanged += layoutSpells;
        }
    }

    public override void forceUpdate()
    {
        layoutSpells(player.Lineup);
        foreach (SpellDisplayer so in spellGOMap.Values)
        {
            so.forceUpdate();
        }
    }

    private void layoutSpells(List<SpellContext> spells)
    {
        //Remove spells no longer in the list
        List<SpellContext> spellsToRemove = new List<SpellContext>();
        foreach (SpellContext spell in spellGOMap.Keys)
        {
            if (!spells.Contains(spell))
            {
                spellsToRemove.Add(spell);
            }
        }
        spellsToRemove.ForEach(spell =>
        {
            SpellDisplayer so = spellGOMap[spell];
            callOnDisplayerDestroyed(so);
            Destroy(so.gameObject);
            spellGOMap.Remove(spell);
        });
        //
        //Create the new spell objects
        List<SpellContext> newSpells = spells.FindAll(spell => !spellGOMap.ContainsKey(spell));
        foreach (SpellContext spell in newSpells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellDisplayer.setUIVars(uiVars);
            spellGOMap.Add(spell, spellDisplayer);
            spellDisplayer.init(spell, player);
            callOnDisplayerCreated(spellDisplayer);
        }
        //Arrange the spell objects
        int flip = (flipped) ? -1 : 1;
        int maxCastingSpeed = Mathf.Max(player.castingSpeed, player.opponent.castingSpeed);
        int x = flip * -1 * (maxCastingSpeed) * buffer / 2;
        mageHoodCoin.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
        x += flip * buffer;
        foreach (SpellDisplayer so in spellGOMap.Values)
        {
            so.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
            x += flip * buffer;
        }
    }
}

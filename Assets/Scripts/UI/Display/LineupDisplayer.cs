using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineupDisplayer : PlayerDisplayUI
{
    public int buffer = 250;
    public bool flipped = false;

    public GameObject mageHoodCoin;

    public GameObject spellPrefab;
    public GameObject emptyPrefab;

    private Dictionary<SpellContext, SpellDisplayer> spellGOMap = new Dictionary<SpellContext, SpellDisplayer>();
    private List<SpellDisplayer> spellGOList = new List<SpellDisplayer>();

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
        spellGOList.FindAll(so => so.spellContext != null)
            .ForEach(so => so.forceUpdate());
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
            spellGOList.Remove(so);
        });
        //Remove tailing nulls
        while (spellGOList.Count > 0
            && spellGOList[spellGOList.Count - 1].spellContext == null
            )
        {
            SpellDisplayer so = spellGOList[spellGOList.Count - 1];
            Destroy(so.gameObject);
            spellGOList.Remove(so);
        }
        //Create the new spell objects
        List<SpellContext> newSpells = spells.FindAll(spell => !spellGOMap.ContainsKey(spell));
        foreach (SpellContext spell in newSpells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellDisplayer.setUIVars(uiVars);
            spellGOMap.Add(spell, spellDisplayer);
            spellGOList.Add(spellDisplayer);
            spellDisplayer.init(spell, player);
            callOnDisplayerCreated(spellDisplayer);
        }
        //Add in tailing nulls
        while (spellGOList.Count < player.castingSpeed)
        {
            GameObject spellObject = Instantiate(emptyPrefab, transform);
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellGOList.Add(spellDisplayer);
        }
        //Remove extra spell displayers (in case some got pushed off the edge)
        while(spellGOList.Count > player.castingSpeed)
        {
            SpellDisplayer so = spellGOList[spellGOList.Count - 1];
            if (so.spellContext != null)
            {
                callOnDisplayerDestroyed(so);
                spellGOMap.Remove(so.spellContext);
            }
            Destroy(so.gameObject);
            spellGOList.Remove(so);
        }
        //Arrange the spell objects
        int flip = (flipped) ? -1 : 1;
        int maxCastingSpeed = Mathf.Max(player.castingSpeed, player.opponent.castingSpeed);
        int x = flip * -1 * (maxCastingSpeed) * buffer / 2;
        mageHoodCoin.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
        x += flip * buffer;
        foreach (SpellDisplayer so in spellGOList)
        {
            so.GetComponent<RectTransform>().localPosition = new Vector2(x, 0);
            x += flip * buffer;
        }
    }
}

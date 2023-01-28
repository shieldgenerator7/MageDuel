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
    private List<SpellDisplayer> empties = new List<SpellDisplayer>();

    protected override void _registerDelegates(bool register)
    {
        if (player)
        {
            player.onLineupChanged -= layoutSpells;
        }
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
            if (spellGOMap.ContainsKey(spell))
            {
                SpellDisplayer so = spellGOMap[spell];
                spellGOMap.Remove(spell);
                Destroy(so.gameObject);
                callOnDisplayerDestroyed(so);
            }
            else
            {
                Debug.LogError($"spell {spell.spell.name} not found in spellGOMap");
            }
        });
        //Hide empties
        empties.ForEach(empty => empty.gameObject.SetActive(false));
        //Create the new spell objects
        List<SpellContext> newSpells = spells.FindAll(spell =>
            spell != null && !spellGOMap.ContainsKey(spell)
        );
        foreach (SpellContext spell in newSpells)
        {
            GameObject spellObject = Instantiate(spellPrefab, transform);
            spellObject.transform.up = Vector3.up;
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            spellDisplayer.setUIVars(uiVars);
            spellGOMap.Add(spell, spellDisplayer);
            spellDisplayer.init(spell);
            callOnDisplayerCreated(spellDisplayer);
        }
        //Generate empties
        int emptiesNeeded = player.castingSpeed - spells.Count(spell => spell != null);
        while (empties.Count < emptiesNeeded)
        {
            GameObject spellObject = Instantiate(emptyPrefab, transform);
            spellObject.transform.up = Vector3.up;
            SpellDisplayer spellDisplayer = spellObject.GetComponent<SpellDisplayer>();
            empties.Add(spellDisplayer);
        }
        //Generate list of spells in order
        int emptyIndex = 0;
        List<SpellDisplayer> spellGOList = new List<SpellDisplayer>();
        for (int i = 0; i < player.castingSpeed; i++)
        {
            //Empty: out of bounds of spells (auto-fill to end with empties)
            if (i >= spells.Count)
            {
                spellGOList.Add(empties[emptyIndex]);
                emptyIndex++;
                continue;
            }
            //
            SpellContext spell = spells[i];
            //Empty: current spell is null
            if (spell == null)
            {
                spellGOList.Add(empties[emptyIndex]);
                emptyIndex++;
                continue;
            }
            //Add spell
            spellGOList.Add(spellGOMap[spell]);
        }
        //Show empties
        for (int i = 0; i < emptyIndex; i++)
        {
            empties[i].gameObject.SetActive(true);
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

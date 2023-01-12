using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplayerUI : PlayerDisplayUI
{
    public int pageIndex;
    private Spell spell;

    public TMP_Text txtName;
    public TMP_Text txtDescription;
    public List<Image> imgColors;
    public List<GameObject> goToggleList;

    private bool pageExists = true;

    protected override void _registerDelegates(bool register)
    {
        pageExists = pageIndex < player.deck.spellList.Count;
        if (pageExists)
        {
            spell = player.deck[pageIndex];
        }
        gameObject.SetActive(pageExists);
    }

    public override void forceUpdate()
    {
        if (!pageExists) { return; }
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        txtDescription.text = spell.Description;
        showToggles(false);
    }

    public void showToggles(bool show)
    {
        goToggleList.ForEach(go => go.SetActive(show));
    }

}

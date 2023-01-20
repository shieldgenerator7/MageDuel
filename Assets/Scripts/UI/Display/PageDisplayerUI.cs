using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class PageDisplayerUI : PlayerDisplayUI
{
    public int pageIndex;
    private Spell spell;

    public TMP_Text txtName;
    public Image imgIcon;
    public TMP_Text txtDescription;
    public TMP_Text txtCost;
    public TMP_Text txtSpeed;
    public List<Image> imgColors;
    public List<GameObject> goToggleList;

    protected override void _registerDelegates(bool register)
    {
        player.onDeckChanged -= onDeckChanged;
        if (register)
        {
            player.onDeckChanged += onDeckChanged;
        }
    }

    public override void forceUpdate()
    {
        updateCircle();
        showToggles(false);
    }

    public void showToggles(bool show)
    {
        goToggleList.ForEach(go => go.SetActive(show));
    }

    private void updateCircle()
    {
        bool pageExists = pageIndex < player.Deck.spellList.Count;
        if (pageExists)
        {
            spell = player.Deck[pageIndex];
        }
        gameObject.SetActive(pageExists);

        if (!pageExists) { return; }
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        imgIcon.sprite = spell.icon;
        txtDescription.text = spell.Description;
        txtCost.text = $"{spell.cost}";
        txtSpeed.text = $"{spell.speed}";
    }

    private void onDeckChanged(Deck deck)
    {
        updateCircle();
    }

}

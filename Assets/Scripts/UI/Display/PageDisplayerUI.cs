using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class PageDisplayerUI : PlayerDisplayUI
{
    private SpellContext spellContext;
    private Spell spell;

    public TMP_Text txtName;
    public Image imgIcon;
    public TMP_Text txtDescription;
    public TMP_Text txtCost;
    public TMP_Text txtSpeed;
    public List<Image> imgColors;

    public void init(SpellContext spellContext)
    {
        this.spellContext = spellContext;
        this.spell = spellContext.spell;
        forceUpdate();
    }
    public void init(Spell spell)
    {
        this.spellContext = null;
        this.spell = spell;
        forceUpdate();
    }

    protected override void _registerDelegates(bool register)
    {
        if (spellContext != null)
        {

        }
        if (register)
        {
            if (spellContext != null)
            {

            }
        }
    }

    public override void forceUpdate()
    {
        updateCircle();
        showPage(false);
    }

    public void showPage(bool show)
    {
        gameObject.SetActive(show);
    }

    private void updateCircle()
    {
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        imgIcon.sprite = spell.icon;
        txtDescription.text = spellContext?.Description ?? spell.Description;
        txtCost.text = $"{spell.cost}";
        txtSpeed.text = $"{spellContext?.Speed ?? spell.speed}";
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplayerUI : PlayerDisplayUI
{
    public int pageIndex;
    private Spell spell;

    public List<Image> imgColors;
    public TMP_Text txtName;
    public TMP_Text txtDescription;

    protected override void _registerDelegates(bool register)
    {
        spell = player.deck[pageIndex];
    }

    public override void forceUpdate()
    {
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        txtDescription.text = spell.Description;
    }

}

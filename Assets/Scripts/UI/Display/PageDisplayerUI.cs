using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplayerUI : PlayerDisplayUI
{
    public Spell spell;

    public List<Image> imgColors;
    public TMP_Text txtName;
    public TMP_Text txtDescription;

    protected override void _registerDelegates(bool register)
    {
        Debug.LogWarning("Not implemented in PageDisplayerUI");
    }

    public override void forceUpdate()
    {
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        txtDescription.text = spell.Description;
    }

}

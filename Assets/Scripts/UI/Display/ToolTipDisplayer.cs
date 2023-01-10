using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipDisplayer : PlayerDisplayUI
{
    public SpellContext spellContext;

    public List<Image> imgColors;
    public TMP_Text txtName;
    public TMP_Text txtDescription;

    protected override void _registerDelegates(bool register)
    {
        spellContext.onAuraChanged -= update;
        spellContext.onFocusChanged -= update;
        if (register)
        {
            spellContext.onAuraChanged += update;
            spellContext.onFocusChanged += update;
        }
    }
    public override void forceUpdate()
    {
        update();
    }

    private void update(int value = 0)
    {
        imgColors.ForEach(img => img.color = spellContext.element.color);
        txtName.text = spellContext.spell.name;
        txtDescription.text = spellContext.Description;
    }

}

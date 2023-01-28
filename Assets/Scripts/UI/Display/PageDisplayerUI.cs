using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    }
    public void init(Spell spell)
    {
        this.spellContext = null;
        this.spell = spell;
    }

    protected override void _registerDelegates(bool register)
    {
        if (spellContext != null)
        {
            spellContext.onAuraChanged -= updateCircle;
            spellContext.onFocusChanged -= updateCircle;
        }
        if (register)
        {
            if (spellContext != null)
            {
                spellContext.onAuraChanged += updateCircle;
                spellContext.onFocusChanged += updateCircle;
            }
        }
    }

    public override void forceUpdate()
    {
        updateCircle();
    }

    public void showPage(bool show)
    {
        gameObject.SetActive(show);
    }

    private void updateCircle(int value = 0)
    {
        imgColors.ForEach(img => img.color = spell?.element.color ?? Color.white);
        txtName.text = spell?.name ?? "no spell";
        imgIcon.sprite = spell?.icon;
        txtDescription.text = spellContext?.Description ?? spell?.Description ?? "no spell desc";
        txtCost.text = $"{spell?.cost ?? 0}";
        txtSpeed.text = $"{spellContext?.Speed ?? spell?.speed ?? 0}";
    }

}

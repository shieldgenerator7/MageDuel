using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageDisplayerUI : SpellDisplayUI
{
    public TMP_Text txtName;
    public Image imgIcon;
    public TMP_Text txtDescription;
    public TMP_Text txtCost;
    public TMP_Text txtSpeed;
    public List<Image> imgColors;

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
        bool revealed = Revealed;
        //Color
        Color color = (revealed)
            ? spell?.element.color ?? Color.white
            : Color.white;
        imgColors.ForEach(img => img.color = color);
        //Title
        txtName.text = (revealed)
            ? spell?.name ?? "no spell"
            : "Hidden";
        //Icon
        imgIcon.enabled = revealed;
        imgIcon.sprite = (revealed)
            ? spell?.icon
            : null;
        //Description
        txtDescription.text = (revealed)
            ? spellContext?.Description ?? spell?.Description ?? "no spell desc"
            : "";
        //Cost
        txtCost.text = (revealed)
            ? $"{spell?.cost ?? 0}"
            : "?";
        //Speed
        txtSpeed.text = (revealed)
            ? $"{spellContext?.Speed ?? spell?.speed ?? 0}"
            : "?";
    }

}

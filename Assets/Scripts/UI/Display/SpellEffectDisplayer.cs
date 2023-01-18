using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellEffectDisplayer : PlayerDisplayUI
{
    [Range(0f, 1f)]
    public float dodgeAlpha = 0.5f;

    public Image imgShield;
    public List<Image> imgDodgeList;
    public Image imgStatic;

    protected override void _registerDelegates(bool register)
    {
        player.onSpellEffectsChanged -= showEffects;
        if (register)
        {
            player.onSpellEffectsChanged += showEffects;
        }
    }

    public override void forceUpdate()
    {
        showEffects(player.SpellEffects);
    }

    private void showEffects(List<SpellEffect> effects)
    {
        showShield(effects.Any(effect => effect is BlockSpellEffect));
        showDodge(effects.Any(effect => effect is Dodge));
        showStatic(effects.Any(effect => effect is UndodgableElement));
    }

    private void showShield(bool show)
    {
        imgShield.gameObject.SetActive(show);
    }
    private void showDodge(bool show)
    {
        float alpha = (show) ? dodgeAlpha : 1;
        imgDodgeList.ForEach(img =>
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        });
    }
    private void showStatic(bool show)
    {
        imgStatic.gameObject.SetActive(show);
    }

}
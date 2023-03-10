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
    public Image imgDodge;
    public List<Image> imgDodgeList;
    public List<Image> imgDodgeHideList;
    public Image imgStatic;

    protected override void _registerDelegates(bool register)
    {
        if (player)
        {
            player.onSpellEffectsChanged -= showEffects;
        }
        if (register)
        {
            player.onSpellEffectsChanged += showEffects;
        }
    }

    public override void forceUpdate()
    {
        showEffects(player.ScriptTokens);
    }

    private void showEffects(List<ScriptToken> effects)
    {
        showShield(effects.Any(effect => effect.isType<BlockSpellEffect>() || effect.isType<AdjustDamageTaken>()));
        showDodge(effects.Any(effect => effect.isType<Dodge>()));
        showStatic(effects.Any(effect => effect.isType<UndodgableElement>()));
    }

    private void showShield(bool show)
    {
        imgShield.gameObject.SetActive(show);
    }
    private void showDodge(bool show)
    {
        //Show dodge image
        imgDodge.gameObject.SetActive(show);
        //Make some images transparent
        float alpha = (show) ? dodgeAlpha : 1;
        imgDodgeList.ForEach(img =>
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        });
        //Hide some images
        imgDodgeHideList.ForEach(img => img.gameObject.SetActive(!show));
    }
    private void showStatic(bool show)
    {
        imgStatic.gameObject.SetActive(show);
    }

}

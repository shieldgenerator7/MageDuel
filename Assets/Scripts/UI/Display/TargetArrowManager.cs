using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetArrowManager : MonoBehaviour
{
    public UIVariables uiVars;
    public GameObject arrowPrefab;
    public Canvas canvas;

    private List<TargetArrow> arrows = new List<TargetArrow>();
    private List<TargetArrowDisplayer> arrowDisplayers = new List<TargetArrowDisplayer>();

    private void Start()
    {
        registerDelegates(true);
        updateToTargetingSpell(uiVars.CurrentTargetingSpell);
    }

    public void registerDelegates(bool register)
    {
        uiVars.onCurrentCastingSpellChanged -= updateToTargetingSpell;
        uiVars.game.onQueueChanged -= updateToCastingSpell;
        uiVars.game.onSubPhaseChanged -= updateToCastingSpell;
        if (register)
        {
            uiVars.onCurrentCastingSpellChanged += updateToTargetingSpell;
            uiVars.game.onQueueChanged += updateToCastingSpell;
            uiVars.game.onSubPhaseChanged += updateToCastingSpell;
        }
    }

    private void updateToTargetingSpell(SpellContext spellContext)
    {
        removeAllArrows();
        if (spellContext != null)
        {
            TargetArrow arrow = makeArrow(spellContext, () => Input.mousePosition);
            addArrow(arrow, true);
            //
            spellContext.onTargetChanged += (targets) =>
            {
                SpellContext target = targets[targets.Count - 1];
                Vector2 endPos = getPosition(target);
                TargetArrow arrow1 = makeArrow(spellContext, () => endPos, 0.5f);
                addArrow(arrow1, true);
                updateArrowDisplayers();
            };
        }
        updateArrowDisplayers();
    }

    private void updateToCastingSpell(List<SpellContext> spells)
    {
        removeAllArrows();
        if (uiVars.game.Phase == Game.GamePhase.MATCHUP && uiVars.game.SubPhase == Game.GameSubPhase.PROCESSING)
        {
            SpellContext spell = spells.FirstOrDefault();
            if (spell != null)
            {
                Vector2 endPos = getPosition(spell.target);
                TargetArrow arrow = makeArrow(spell, () => endPos);
                addArrow(arrow, true);
                //Make target arrows
                foreach(SpellContext target in spell.SpellTargets)
                {
                    Vector2 endPos1 = getPosition(target);
                    TargetArrow arrow1 = makeArrow(spell, () => endPos1, 0.5f);
                    addArrow(arrow1, true);
                }
                //Focus target spell arrows more
                if (arrows.Count > 1)
                {
                    arrows.ForEach(arr => arr.color.a = 1);
                    arrow.color.a = 0.5f;
                }
            }
        }
        updateArrowDisplayers();
    }
    private void updateToCastingSpell(Game.GameSubPhase subphase)
    {
        updateToCastingSpell(uiVars.game.CastingQueue);
    }

    private TargetArrow makeArrow(SpellContext spellContext, Func<Vector2> endFunc, float alpha = 1)
    {
        Vector3 startPos = getPosition(spellContext);
        Color elementColor = spellContext.spell.element.color;
        elementColor.a = alpha;
        TargetArrow arrow = new TargetArrow(
            startPos,
            endFunc,
            elementColor
            );
        return arrow;
    }
    private Vector2 getPosition(SpellContext spellContext)
    {
        return FindObjectsOfType<SpellDisplayer>()
                .First(so => so.SpellContext == spellContext)
                .transform.position;
    }
    private Vector2 getPosition(Player player)
    {
        return FindObjectsOfType<PlayerPoolDisplayer>()
                .First(ppd => ppd.Player == player)
                .transform.position;
    }

    private void addArrow(TargetArrow arrow, bool add)
    {
        if (add)
        {
            if (!arrows.Contains(arrow))
            {
                arrows.Add(arrow);
            }
        }
        else
        {
            arrows.Remove(arrow);
        }
    }
    private void removeAllArrows()
    {
        arrows.Clear();
    }

    private void updateArrowDisplayers()
    {
        //Hide existing displayers
        arrowDisplayers.ForEach(arrow => arrow.showArrow(false));
        //Make as many as needed
        while (arrowDisplayers.Count < arrows.Count)
        {
            GameObject go = Instantiate(arrowPrefab, canvas.transform);
            TargetArrowDisplayer arrowDisplayer = go.GetComponent<TargetArrowDisplayer>();
            arrowDisplayer.showArrow(false);
            arrowDisplayers.Add(arrowDisplayer);
        }
        //Set the ones needed
        for (int i = 0; i < arrows.Count; i++)
        {
            arrowDisplayers[i].init(arrows[i]);
        }
    }
}

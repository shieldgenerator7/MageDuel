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
        updateToCastingSpell(uiVars.CurrentTargetingSpell);
    }

    public void registerDelegates(bool register)
    {
        uiVars.onCurrentCastingSpellChanged -= updateToCastingSpell;
        if (register)
        {
            uiVars.onCurrentCastingSpellChanged += updateToCastingSpell;
        }
    }

    private void updateToCastingSpell(SpellContext spellContext)
    {
        if (spellContext != null)
        {
            Vector3 startPos = FindObjectsOfType<SpellDisplayer>()
                .First(so => so.spellContext == spellContext)
                .transform.position;
            TargetArrow arrow = new TargetArrow(
                startPos,
                () => Input.mousePosition - startPos,
                spellContext.spell.element.color
                );
            addArrow(arrow, true);
        }
        else
        {
            //Remove all arrows
            while (arrows.Count > 0)
            {
                addArrow(arrows[0], false);
            }
        }
        updateArrowDisplayers();
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

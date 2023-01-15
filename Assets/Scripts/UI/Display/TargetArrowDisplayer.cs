using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetArrowDisplayer : MonoBehaviour
{
    public UIVariables uiVars;
    public Image imgArrow;

    private RectTransform rectTransform;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        registerDelegates(true);
        updateToCastingSpell(uiVars.CurrentCastingSpell);
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
        active = spellContext != null;
        if (active)
        {
            updatePosition();
            imgArrow.color = spellContext.spell.element.color;
            transform.position = FindObjectsOfType<SpellDisplayer>()
                .First(so => so.spellContext == spellContext)
                .transform.position;
        }
        imgArrow.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (active)
        {
            updatePosition();
        }
    }

    private void updatePosition()
    {
        Vector2 pointer = Input.mousePosition - transform.position;
        transform.up = pointer;
        Vector2 size = rectTransform.sizeDelta;
        size.y = pointer.magnitude * 2;
        rectTransform.sizeDelta = size;
    }
}

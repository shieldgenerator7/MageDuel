using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    public SpellControllerUI spellControllerUI;
    public SpellDisplayer spellDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        spellControllerUI.spellContext = spellDisplayer.SpellContext;
        spellControllerUI.setPlayer(spellDisplayer.Player, spellDisplayer.UIVars);
    }
}

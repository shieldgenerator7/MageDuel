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
        spellControllerUI.spellContext = spellDisplayer.spellContext;
        spellControllerUI.setPlayer(spellDisplayer.Player, spellDisplayer.UIVars);
    }
}

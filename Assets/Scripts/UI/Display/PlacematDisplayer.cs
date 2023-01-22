using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlacematDisplayer : MonoBehaviour
{

    private List<PlayerDisplayUI> uiElements = new List<PlayerDisplayUI>();

    private Player player;
    private UIVariables uiVars;

    private void Start()
    {
        refreshChildren();
    }

    public void setPlayer(Player player, UIVariables uiVars)
    {
        //UIVars
        this.uiVars = uiVars;
        uiElements.ForEach(ui =>
        {
            ui.setUIVars(uiVars);
        });
        //
        registerUIChanges();
        //Unlink from previous player
        if (this.player)
        {
            registerDelegates(false);
        }
        //Set player
        this.player = player;
        //Link to new player
        if (this.player)
        {
            registerDelegates(true);
        }
    }

    public void refreshChildren()
    {
        uiElements = GetComponentsInChildren<PlayerDisplayUI>(true).ToList();
        setPlayer(player, uiVars);
    }

    void OnEnable()
    {
        registerUIChanges();
        if (this.player)
        {
            registerDelegates(true);
        }
    }
    private void OnDisable()
    {
        if (this.player)
        {
            registerDelegates(false);
        }
    }

    private void registerDelegates(bool register)
    {
        if (register)
        {
            uiElements.ForEach(ui =>
            {
                ui.registerDelegates(player);
                ui.forceUpdate();
            });
        }
        else
        {
            uiElements.ForEach(ui => ui.registerDelegates(player, false));
        }
    }

    private void registerUIChanges()
    {
        uiElements.ForEach(ui => registerUIChange(ui));
    }
    private void registerUIChange(PlayerDisplayUI playerDisplayUI)
    {
        playerDisplayUI.onDisplayerCreated -= updateUIAfterChange;
        playerDisplayUI.onDisplayerCreated += updateUIAfterChange;
        playerDisplayUI.onDisplayerDestroyed -= updateUIAfterChange;
        playerDisplayUI.onDisplayerDestroyed += updateUIAfterChange;
    }

    private void updateUIAfterChange(PlayerDisplayUI playerDisplayUI)
    {
        refreshChildren();
        registerUIChanges();
        onUIChanged?.Invoke();
    }
    public delegate void OnUIChanged();
    public event OnUIChanged onUIChanged;
}

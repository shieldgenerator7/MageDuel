using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public Deck defaultDeck;
    public List<string> playerNames;
    public List<Color> playerColors;
    public List<Deck> decks;
}

using System.Collections.Generic;

public class Spell
{
    public string name;
    public string description;
    public Element element;
    public int cost;
    public int speed = 3;
    //public int strainCost = 0;
    public List<SpellEffect> effects = new List<SpellEffect>();
}
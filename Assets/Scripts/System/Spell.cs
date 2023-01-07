using System.Collections.Generic;

[System.Serializable]
public class Spell
{
    public string name;
    public string description;
    public Element element;
    public int cost;
    public int speed = 3;
    //public int strainCost = 0;
    public List<SpellEffect> effects = new List<SpellEffect>();
    public List<SpellAttribute> attributes = new List<SpellAttribute>();

    public int getAttribute(string attrName)
    {
        foreach (SpellAttribute attr in attributes)
        {
            if (attr.name == attrName)
            {
                return attr.value;
            }
        }
        return 0;
    }
}
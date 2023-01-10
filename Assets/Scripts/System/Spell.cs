using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell: ScriptableObject
{
    public new string name;
    [TextArea(2,10)]
    public string description;
    public Element element;
    public int cost;
    public int speed = 3;
    //public int strainCost = 0;
    public bool autoTargetEnemy = true;
    public bool autoTargetSelf = false;
    public List<SpellEffect> effects = new List<SpellEffect>();
    public List<SpellAttribute> attributes = new List<SpellAttribute>();
    [Multiline(50)]
    public string script;

    public SpellAttribute getAttribute(string attrName)
    {
        foreach (SpellAttribute attr in attributes)
        {
            if (attr.name == attrName)
            {
                return attr;
            }
        }
        Debug.LogError($"Attribute with name {attrName} not found! spell: {this.name}");
        return null;
    }

    public string Description
    {
        get
        {
            string desc = this.description;
            foreach(SpellAttribute attr in attributes)
            {
                desc = desc.Replace($"[{attr.name}]", $"{attr.value}");
            }
            return desc;
        }
    }
}
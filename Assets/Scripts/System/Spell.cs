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
    public List<Keyword> keywords;
    public AttributeSet attributes;
    [TextArea(5, 50)]
    public string script;

    public enum Keyword
    {
        FLASH,
        RECAST, //not needed in the digital version?
        CHANNEL,
        PERSISTS,
        MASTER, //ultimate
    }

    public string Description
    {
        get
        {
            string desc = this.description;
            //Find vars in desc
            List<string> vars = new List<string>();
            int startBrak = desc.IndexOf('[');
            while (startBrak > 0)
            {
                int endBrak = desc.IndexOf(']', startBrak);
                vars.Add(desc.Substring(startBrak + 1, endBrak - startBrak - 1));
                startBrak = desc.IndexOf('[', endBrak);
            }
            foreach (string attrName in vars)
            {
                desc = desc.Replace($"[{attrName}]", $"{attributes.get(attrName)}");
            }
            return desc;
        }
    }
}
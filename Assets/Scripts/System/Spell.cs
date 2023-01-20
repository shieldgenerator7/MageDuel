using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public new string name;
    public Sprite icon;
    [TextArea(2, 10)]
    public string description;
    public Element element;
    [Range(0, 15)]
    public int cost;
    [Range(0, 5)]
    public int speed = 3;
    //public int strainCost = 0;
    public bool autoTargetEnemy = true;
    public bool autoTargetSelf = false;
    public List<Keyword> keywords;
    public AttributeSet attributes;
    public List<SpellTarget> spellTargets;
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
            string keywordStr = "";
            foreach (Keyword keyword in keywords)
            {
                keywordStr += $"<b>{keyword}</b> ";
            }
            if (!string.IsNullOrEmpty(keywordStr))
            {
                keywordStr += "\n";
            }
            desc = keywordStr + desc;
            return desc;
        }
    }
}
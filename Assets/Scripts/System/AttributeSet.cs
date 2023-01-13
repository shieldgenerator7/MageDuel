using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeSet
{
    [SerializeField]
    private List<SpellAttribute> attributes = new List<SpellAttribute>();
    private AttributeSet backup;

    public AttributeSet(AttributeSet backup)
    {
        this.backup = backup;
    }

    public int get(string name)
        => getAttribute(name)?.value ?? 0;

    public SpellAttribute getAttribute(string name)
        => attributes.Find(attr => attr.name == name)
        ?? backup?.getAttribute(name)
        ?? null;

    public void set(string name, int value)
    {
        SpellAttribute attr = attributes.Find(attr => attr.name == name);
        if (attr == null)
        {
            attr = new SpellAttribute()
            {
                name = name,
                value = value,
                rampable = false,
            };
            attributes.Add(attr);
        }
        attr.value = value;
    }
}

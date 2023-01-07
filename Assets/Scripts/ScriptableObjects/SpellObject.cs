using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SpellObject",menuName ="SpellObject")]
public class SpellObject : ScriptableObject
{
    public Spell spell;
    [Multiline(50)]
    public string script;
}

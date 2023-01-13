using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableSet
{
    private Dictionary<string, int> variables = new Dictionary<string, int>();
    private VariableSet backup;

    public VariableSet(VariableSet backup = null)
    {
        this.backup = backup;
    }

    public int get(string name)
    {
        if (variables.ContainsKey(name))
        {
            return variables[name];
        }
        if (backup != null)
        {
            return backup.get(name);
        }
        return 0;
    }

    public void set(string name, int value)
    {
        variables[name] = value;
    }
}

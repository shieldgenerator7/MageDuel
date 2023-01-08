using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Element")]
public class Element : ScriptableObject
{
    public new string name;
    public Color color = Color.white;
}
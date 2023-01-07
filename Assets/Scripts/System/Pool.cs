using UnityEngine;

public class Pool
{
    public int minValue;
    public int maxValue;

    private int value;
    public int Value
    {
        get => this.value;
        set => this.value = Mathf.Clamp(value, minValue, maxValue);
    }

    public Pool(int max) : this(0, max) { }
    public Pool(int min, int max)
    {
        this.minValue = min;
        this.maxValue = max;
        this.value = this.maxValue;
    }
}
using UnityEngine;

public sealed class Pool
{
    public int minValue;
    public int maxValue;

    private int value;
    public int Value
    {
        get => this.value;
        set
        {
            this.value = Mathf.Clamp(value, minValue, maxValue);
            onValueChanged?.Invoke(this.value);
            if (this.value == minValue)
            {
                onValueMinned?.Invoke(this.value);
            }
            if (this.value == maxValue)
            {
                onValueMaxxed?.Invoke(this.value);
            }
        }
    }
    public delegate void OnValueChanged(int value);
    public event OnValueChanged onValueChanged;
    public event OnValueChanged onValueMinned;
    public event OnValueChanged onValueMaxxed;

    public Pool(int max) : this(0, max) { }
    public Pool(int min, int max)
    {
        this.minValue = min;
        this.maxValue = max;
        this.value = this.maxValue;
    }

    public static implicit operator int(Pool pool) => pool.Value;
    public static int operator +(Pool pool, int value) => pool.Value + value;
    public static int operator +(int value, Pool pool) => value + pool.Value;
    public static int operator -(Pool pool, int value) => pool.Value - value;
    public static int operator -(int value, Pool pool) => value - pool.Value;
}
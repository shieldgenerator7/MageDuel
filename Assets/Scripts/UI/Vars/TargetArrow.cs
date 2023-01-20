using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow
{
    public Vector2 startPos;
    public Func<Vector2> endPos;
    public Color color;

    public Vector2 EndPos => endPos();

    public Vector2 Direction => EndPos - startPos;

    public TargetArrow(Vector2 startPos, Func<Vector2> endPos)
        : this(startPos, endPos, Color.white) { }
    public TargetArrow(Vector2 startPos, Func<Vector2> endPos, Color color)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.color = color;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2Int() { }
    
    public override bool Equals(object obj)
    {
        if(obj == null || obj.GetType() != typeof(Vector2Int)) return false;
        return ((Vector2Int) obj).x == x && ((Vector2Int)obj).y == y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}

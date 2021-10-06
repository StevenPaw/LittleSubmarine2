using UnityEngine;

public class Oneway : SpecialTile
{
    public Vector2 GetDirection()
    {
        return transform.up;
    }
}

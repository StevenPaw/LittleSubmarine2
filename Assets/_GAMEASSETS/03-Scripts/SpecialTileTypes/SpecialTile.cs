using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    [SerializeField] private SpecialTileTypes tileType;

    public SpecialTileTypes GetTileType()
    {
        return tileType;
    }
}

public enum SpecialTileTypes
{
    NONE,
    PUSHABLE,
    ONEWAY,
    CONVEYERBELT,
    DEADLY
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    [SerializeField] private SpecialTileTypes tileType;

    public SpecialTileTypes GetType()
    {
        return tileType;
    }
}

public enum SpecialTileTypes
{
    NONE,
    ONEWAY,
    CONVEYERBELT
}

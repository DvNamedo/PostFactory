using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game/Tile Type Database")]
public class TileTypeDatabase : ScriptableObject
{
    [System.Serializable]
    public class TileEntry
    {
        public TileBase tile;
        public TileType type;
        public placetype placeType;
        public enum placetype
        {
                Unable = 0,
                NoResource = 1,
                Stone = 2,
                Wood = 3
        } 
    }

    public TileEntry[] tiles;

    Dictionary<TileBase, TileEntry> lookup;

    public void Init()
    {
        lookup = new Dictionary<TileBase, TileEntry>();

        foreach (var e in tiles)
        {
            if (e.tile != null && !lookup.ContainsKey(e.tile))
                lookup.Add(e.tile, e);
        }
    }

    public TileType GetType(TileBase tile)
    {
        if (tile == null) return TileType.Empty;

        if (lookup.TryGetValue(tile, out var entry))
            return entry.type;

        return TileType.Empty;
    }
    public TileEntry GetEntryInternal(TileBase tile)
    {
        if (tile == null) return null;

        lookup.TryGetValue(tile, out var entry);
        return entry;
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileReader : MonoBehaviour
{
    public Tilemap tilemap;
    public TileTypeDatabase database;

    void Awake()
    {
        database.Init();
    }

    public TileType GetTileType(Vector2Int pos)
    {
        TileBase tile = tilemap.GetTile((Vector3Int)pos);
        return database.GetType(tile);
    }

    public TileTypeDatabase.TileEntry.placetype PlaceType(Vector2Int pos)
    {
        TileBase tile = tilemap.GetTile((Vector3Int)pos);
        var entry = database.GetEntryInternal(tile);
        return entry != null ? entry.placeType : TileTypeDatabase.TileEntry.placetype.Unable;
    }
}
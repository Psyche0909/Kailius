using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundary : MonoBehaviour
{
    public float wallThickness = 2f;

    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>();
        }
        if (tilemap == null) return;

        CreateBoundaryWalls(tilemap);
    }

    void CreateBoundaryWalls(Tilemap tilemap)
    {
        Bounds bounds = tilemap.localBounds;
        Vector3 tilemapPos = tilemap.transform.position;

        float minX = tilemapPos.x + bounds.min.x;
        float maxX = tilemapPos.x + bounds.max.x;
        float minY = tilemapPos.y + bounds.min.y;
        float maxY = tilemapPos.y + bounds.max.y;

        GameObject walls = new GameObject("BoundaryWalls");
        walls.transform.SetParent(transform);

        CreateWall(walls, "Wall_Bottom", new Vector2((minX + maxX) / 2f, minY - wallThickness / 2f),
                   new Vector2(maxX - minX, wallThickness));
        CreateWall(walls, "Wall_Top", new Vector2((minX + maxX) / 2f, maxY + wallThickness / 2f),
                   new Vector2(maxX - minX, wallThickness));
        CreateWall(walls, "Wall_Left", new Vector2(minX - wallThickness / 2f, (minY + maxY) / 2f),
                   new Vector2(wallThickness, maxY - minY));
        CreateWall(walls, "Wall_Right", new Vector2(maxX + wallThickness / 2f, (minY + maxY) / 2f),
                   new Vector2(wallThickness, maxY - minY));
    }

    void CreateWall(GameObject parent, string name, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(parent.transform);
        wall.transform.position = position;
        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = size;
    }
}

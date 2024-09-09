using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour {
    
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private Pattern pattern;

    private void Start()
    {
        SetPattern(pattern);
    }

    private void SetPattern(Pattern pattern)
    {
        Clear();

        Vector2Int center = pattern.GetCenter();

        for(int i = 0; i < pattern.cells.Length; i++)
        {
            Vector2Int cell = pattern.cells[i] - center;
            currentState.SetTile((Vector3Int)cell, aliveTile);
        }
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
    }

}
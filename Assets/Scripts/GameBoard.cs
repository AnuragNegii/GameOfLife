using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
public class GameBoard : MonoBehaviour {
    
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private Pattern pattern;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    private void Awake() {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

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
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        while(enabled)
        {
            UpdateState();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();
        
        // Gathering cells To check
        foreach (Vector3Int cell in aliveCells)
        {
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
                }
            }
        }

        // transitioning cells to next stage
        foreach( Vector3Int cell in cellsToCheck)
        {
            int neighbor = CountNeighbours(cell);
            bool alive = IsAlive(cell);

            if(!alive && neighbor == 3)
            {
                //becomes alive
                nextState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }else if(alive && neighbor < 2 || neighbor > 3)
            {
                //becomes dead
                nextState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }else
            {
                //stays the same
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
    }

    private int CountNeighbours(Vector3Int cell)
    {
        int count = 0;    

        for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    Vector3Int neighbor = cell + new Vector3Int(x, y, 0);
                    if(x == 0 && y == 0){
                        continue;
                    }
                    else if(IsAlive(neighbor)){
                        count++;
                    }
                }
            }
        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }
}
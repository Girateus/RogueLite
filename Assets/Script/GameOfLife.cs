using UnityEngine;
using UnityEngine.Tilemaps;

public class GameOfLife : MonoBehaviour
{
   [Header("References")]
    public Tilemap tilemap;
    public TileBase aliveTile;

    [Header("Settings")]
    public Vector2Int gridSize = new Vector2Int(50, 50);
    public float updateInterval = 0.2f;

    private bool[,] currentGen;
    private float timer;

    void Start()
    {
        currentGen = new bool[gridSize.x, gridSize.y];
        RandomizeGrid();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0;
            CalculateNextGeneration();
            DrawGrid();
        }
    }

    void RandomizeGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                currentGen[x, y] = Random.value > 0.85f; // 15% chance to start alive
            }
        }
    }

    void CalculateNextGeneration()
    {
        bool[,] nextGen = new bool[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                int neighbors = CountAliveNeighbors(x, y);
                bool isAlive = currentGen[x, y];

                // --- The Rules of Life ---
                if (isAlive && (neighbors == 2 || neighbors == 3)) 
                    nextGen[x, y] = true;
                else if (!isAlive && neighbors == 3) 
                    nextGen[x, y] = true;
                else 
                    nextGen[x, y] = false;
            }
        }
        currentGen = nextGen;
    }

    int CountAliveNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Skip the cell itself

                int checkX = x + i;
                int checkY = y + j;

                // Border Check
                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    if (currentGen[checkX, checkY]) count++;
                }
            }
        }
        return count;
    }

    void DrawGrid()
    {
        tilemap.ClearAllTiles(); // Wipe the board
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (currentGen[x, y])
                {
                    // Place the tile at the grid coordinate
                    tilemap.SetTile(new Vector3Int(x, y, 0), aliveTile);
                }
            }
        }
    }
}

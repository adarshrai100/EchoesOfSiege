using System.Collections.Generic;
using UnityEngine;

public class PathValidator : MonoBehaviour
{
    [SerializeField] private Vector2Int _startPosition;
    [SerializeField] private Vector2Int _endPosition;

    public Vector2Int StartPosition => _startPosition;   // 👈 ADD THIS
    public Vector2Int EndPosition => _endPosition;       // 👈 ADD THIS

    public bool HasValidPath(GridCell[,] grid, int width, int height)
    {
        Debug.Log("Running BFS validation...");
        GridCell startCell = grid[_startPosition.x, _startPosition.y];
        GridCell endCell = grid[_endPosition.x, _endPosition.y];

        Queue<GridCell> queue = new Queue<GridCell>();
        HashSet<GridCell> visited = new HashSet<GridCell>();

        queue.Enqueue(startCell);
        visited.Add(startCell);

        while (queue.Count > 0)
        {
            GridCell current = queue.Dequeue();

            if (current == endCell)
                return true;

            foreach (GridCell neighbor in GetNeighbors(current, grid, width, height))
            {
                if (!neighbor.IsWalkable || visited.Contains(neighbor))
                    continue;

                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        Debug.Log("Running BFS validation...");
        return false;
    }

    private IEnumerable<GridCell> GetNeighbors(GridCell cell, GridCell[,] grid, int width, int height)
    {
        Vector2Int pos = cell.GridPosition;

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = pos + dir;

            if (neighborPos.x >= 0 && neighborPos.x < width &&
                neighborPos.y >= 0 && neighborPos.y < height)
            {
                yield return grid[neighborPos.x, neighborPos.y];
            }
        }
    }
}
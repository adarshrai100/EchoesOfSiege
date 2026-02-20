using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public bool IsOccupied { get; private set; }

    public void Initialize(Vector2Int position)
    {
        GridPosition = position;
        IsOccupied = false;
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
}
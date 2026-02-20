using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public bool IsOccupied { get; private set; }

    private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
    }

    public void Initialize(Vector2Int position)
    {
        GridPosition = position;
        IsOccupied = false;
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public void Highlight(bool state)
    {
        if (_renderer == null) return;

        _renderer.material.color = state ? Color.green : _defaultColor;
    }
}
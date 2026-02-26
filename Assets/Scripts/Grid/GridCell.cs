using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Color _baseColor;
    private Color _normalColor = Color.white;
    private Color _pathColor = new Color(0.85f, 0.85f, 0.85f);
    private Color _highlightColor = new Color(0.7f, 1f, 0.7f);

    private bool _isPathCell = false;

    public bool IsOccupied { get; private set; }
    public bool IsPathCell => _isPathCell;

    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        _baseColor = _normalColor;
        _renderer.material.color = _baseColor;
    }

    public void Initialize(Vector2Int position)
    {
        GridPosition = position;
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public void SetAsPathCell()
    {
        _isPathCell = true;
        _baseColor = _pathColor;
        _renderer.material.color = _baseColor;
    }

    public void Highlight(bool state)
    {
        if (_renderer == null) return;

        _renderer.material.color = state ? _highlightColor : _baseColor;
    }
}
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _pathMaterial;
    [SerializeField] private Material _highlightMaterial;

    private bool _isPathCell = false;

    public bool IsOccupied { get; private set; }
    public bool IsPathCell => _isPathCell;

    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        _renderer.material = _normalMaterial;
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
        _renderer.material = _pathMaterial;
    }

    public void Highlight(bool state)
    {
        if (_renderer == null) return;

        if (state)
            _renderer.material = _highlightMaterial;
        else
            _renderer.material = _isPathCell ? _pathMaterial : _normalMaterial;
    }
}
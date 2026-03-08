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

    public enum CellVisualState
    {
        Normal,
        Hover,
        Valid,
        Invalid
    }

    public void SetVisualState(CellVisualState state)
    {
        if (_renderer == null) return;

        switch (state)
        {
            case CellVisualState.Normal:
                _renderer.material = _isPathCell ? _pathMaterial : _normalMaterial;
                break;

            case CellVisualState.Hover:
                _renderer.material = _highlightMaterial;
                break;

            case CellVisualState.Valid:
                ApplyTint(new Color32(80, 200, 120, 255)); // soft green
                break;

            case CellVisualState.Invalid:
                ApplyTint(new Color32(220, 80, 80, 255)); // soft red
                break;
        }
    }

    private void ApplyTint(Color tint)
    {
        _renderer.material = _isPathCell ? _pathMaterial : _normalMaterial;

        Color baseColor = _renderer.material.color;
        _renderer.material.color = Color.Lerp(baseColor, tint, 0.6f);
    }
}
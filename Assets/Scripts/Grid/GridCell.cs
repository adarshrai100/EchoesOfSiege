using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Color _defaultColor;
    private Color _validColor = new Color(0.7f, 1f, 0.7f);
    private Color _invalidColor = new Color(1f, 0.4f, 0.4f);

    public bool IsOccupied { get; private set; }
    public bool IsWalkable { get; private set; } = true;

    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        _defaultColor = _renderer.material.color;
    }

    public void Initialize(Vector2Int position)
    {
        GridPosition = position;
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
        IsWalkable = !occupied;
    }

    public void ShowValidPreview()
    {
        _renderer.material.color = _validColor;
    }

    public void ShowInvalidPreview()
    {
        _renderer.material.color = _invalidColor;
    }

    public void ResetColor()
    {
        _renderer.material.color = _defaultColor;
    }
}